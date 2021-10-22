using FlightBot.Conversation;
using FlightBot.Conversation.Extensions;
using FlightBot.Conversation.Factories.Abstractions;
using FlightBot.Services.Abstractions;
using FlightBot.Services.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public sealed class StateManagerService : IStateManagerService
    {
        readonly IAirportFindingService _airportFindingService;
        readonly ILuisInterpreterService _textExtractorService;
        readonly IFlightFindingService _flightFindingService;
        readonly IAdaptiveCardFactory _adaptiveCardFactory;

        public StateManagerService(IAirportFindingService airportFindingService,
            ILuisInterpreterService textExtractorService, IFlightFindingService flightFindingService,
            IAdaptiveCardFactory adaptiveCardFactory)
        {
            _airportFindingService = airportFindingService;
            _textExtractorService = textExtractorService;
            _flightFindingService = flightFindingService;
            _adaptiveCardFactory = adaptiveCardFactory;
        }

        public async Task<Attachment> GenerateCurentState(UserProfile userProfile,
            ConversationData conversationData, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            switch (conversationData.CurrentState)
            {
                case FlightFindingStates.FindAirport:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (conversationData.NearbyAirports == null)
                        {
                            return await FindClosestAirports(conversationData);
                        }
                        else if (userInput.Equals(MessageManager.NO_SUITABLE_FLIGHTS))
                        {
                            return await FindClosestAirports(conversationData, true);
                        }
                        else if (conversationData.NearbyAirports != null && !string.IsNullOrEmpty(userInput))
                        {
                            var searchResult = await _airportFindingService.FindAssociatedAirports(userInput);

                            if (searchResult.Count > 0)
                            {
                                conversationData.NearbyAirports = searchResult;
                                userProfile.SelectedAirport = userInput;

                                conversationData.CurrentState = FlightFindingStates.GetDestination;

                                var message = MessageManager.AIRPORT_CONFIRMED;
                                return _adaptiveCardFactory.GetTextCard(message);
                            }
                            else
                            {
                                var message = MessageManager.AIRPORT_NOT_FOUND;
                                return _adaptiveCardFactory.GetTextCard(message);
                            }
                        }
                    }
                    break;

                case FlightFindingStates.GetDestination:
                    {
                        var destination = await _textExtractorService.InterpretDestination(turnContext, cancellationToken);

                        if (destination.Equals(string.Empty))
                        {
                            var message = MessageManager.DESTINATION_NOT_RECOGNIZED(turnContext.Activity.Text);

                            return _adaptiveCardFactory.GetTextCard(message);
                        }

                        var destinationAirports = await _airportFindingService.FindAssociatedAirports(destination);

                        if (await _flightFindingService.CheckFlightsTo(conversationData.NearbyAirports, destinationAirports))
                        {
                            userProfile.Destination = destination;

                            conversationData.DestinationAirports = destinationAirports;
                            conversationData.CurrentState = FlightFindingStates.GetFlightDate;

                            var message = MessageManager.DESTINATON_CONFIRMED(destination);
                            return _adaptiveCardFactory.GetCalanderCard(message);
                        }
                        else
                        {
                            conversationData.NearbyAirports = null;
                            conversationData.CurrentState = FlightFindingStates.FindAirport;

                            var message = MessageManager.DESTINATION_NOT_AVAILIBLE(userProfile.SelectedAirport, destination);

                            return _adaptiveCardFactory.GetTextCard(message);
                        }
                    }

                case FlightFindingStates.GetFlightDate:
                    {
                        string usersSelectedDate = turnContext.Activity.Value.ToString();
                        var userInput = usersSelectedDate.GetDatefromCalander();
                        var displayDate = userInput.ToShortDateString();

                        if (userInput > DateTime.Now)
                        {
                            if (await _flightFindingService.CheckFlightsToOn(conversationData.NearbyAirports, conversationData.DestinationAirports, userInput))
                            {
                                userProfile.FlightDate = userInput;
                                userProfile.DisplayFlightDate = displayDate;

                                string message = MessageManager.RETURN_FLIGHT_ASK(userProfile.Destination, displayDate);

                                conversationData.CurrentState = FlightFindingStates.GetReturnFlight;

                                return _adaptiveCardFactory.GetOptionalCalanderCard(message);
                            }
                            else
                            {
                                string message = MessageManager.NO_FLIGHTS_FOUND(userProfile.SelectedAirport,
                                    userProfile.Destination, displayDate);

                                return _adaptiveCardFactory.GetCalanderCard(message);
                            }
                        }
                        else
                        {
                            string message = MessageManager.RECONFIRM_DATE(userProfile.Destination, displayDate);

                            return _adaptiveCardFactory.GetCalanderCard(message);
                        }
                    }

                case FlightFindingStates.GetReturnFlight:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (userInput == null)
                        {
                            string usersSelectedDate = turnContext.Activity.Value.ToString();
                            var returnDate = usersSelectedDate.GetDatefromCalander();
                            var displayDate = returnDate.ToShortDateString();

                            if (userProfile.FlightDate >= returnDate)
                            {
                                string message = MessageManager.INVALID_RETURN_DATE(userProfile.DisplayFlightDate, displayDate);

                                return _adaptiveCardFactory.GetCalanderCard(message);
                            }
                            else
                            {
                                var foundFlights = await _flightFindingService.FindFlights(
                                    conversationData.NearbyAirports, conversationData.DestinationAirports,
                                    userProfile.FlightDate, returnDate);

                                if (foundFlights.Count == 0)
                                {
                                    string message = MessageManager.RECONFIRM_DATE(userProfile.Destination, displayDate);

                                    return _adaptiveCardFactory.GetCalanderCard(message);
                                }
                                else
                                {
                                    conversationData.CurrentState = FlightFindingStates.FindAirport;

                                    var message = MessageManager.FOUND_RETURN_FLIGHTS(userProfile.SelectedAirport,
                                        userProfile.Destination, userProfile.DisplayFlightDate, displayDate);

                                    return _adaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                                }
                            }
                        }
                        else
                        {
                            conversationData.CurrentState = FlightFindingStates.FindAirport;

                            var foundFlights = await _flightFindingService.FindFlights(
                                conversationData.NearbyAirports, conversationData.DestinationAirports,
                                userProfile.FlightDate);

                            var message = MessageManager.FOUND_FLIGHTS(userProfile.SelectedAirport,
                                userProfile.Destination, userProfile.DisplayFlightDate);

                            return _adaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                        }
                    }
            }

            return null;
        }

        private async Task<Attachment> FindClosestAirports(ConversationData conversationData, bool isRestarting = false)
        {
            conversationData.NearbyAirports = await _airportFindingService.FindClosestAirports();

            var message = isRestarting ? MessageManager.RESTART_MESSAGE(conversationData.NearbyAirports.Count) :
                MessageManager.WELCOME_MESSAGE(conversationData.NearbyAirports.Count);

            return conversationData.NearbyAirports.Count == 0 ? _adaptiveCardFactory.GetTextCard(message) :
                _adaptiveCardFactory.GetCaroselCard(message, conversationData.NearbyAirports.Select(x => x.AirportName).ToArray());
        }
    }
}
