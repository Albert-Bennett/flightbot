using FlightBot.Conversation;
using FlightBot.Conversation.Extensions;
using FlightBot.Conversation.Factories;
using FlightBot.Services.Abstractions;
using FlightBot.Services.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public sealed class StateManagerService : IStateManagerService
    {
        readonly IAirportFindingService _airportFindingService;
        readonly ITextExtractorService _textExtractorService;
        readonly IFlightFindingService _flightFindingService;

        public StateManagerService(IAirportFindingService airportFindingService,
            ITextExtractorService textExtractorService, IFlightFindingService flightFindingService)
        {
            _airportFindingService = airportFindingService;
            _textExtractorService = textExtractorService;
            _flightFindingService = flightFindingService;
        }

        public async Task<Attachment> GenerateCurentState(UserProfile userProfile,
            ConversationData conversationData, ITurnContext turnContext)
        {
            switch (conversationData.CurrentState)
            {
                case FlightFindingStates.FindAirport:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (conversationData.AirportsFound == null)
                        {
                            return await FindClosestAirports(conversationData);
                        }
                        else if (userInput.Equals(MessageManager.NO_SUITABLE_FLIGHTS)) 
                        {
                            return await FindClosestAirports(conversationData, true);
                        }
                        else if (conversationData.AirportsFound != null && !string.IsNullOrEmpty(userInput))
                        {
                            if (await _airportFindingService.ConfirmAirportExists(userInput))
                            {
                                userProfile.SelectedAirport = userInput;

                                conversationData.CurrentState = FlightFindingStates.GetDestination;

                                var message = MessageManager.AIRPORT_CONFIRMED;
                                return AdaptiveCardFactory.GetTextCard(message);
                            }
                            else
                            {
                                var message = MessageManager.AIRPORT_NOT_FOUND;
                                return AdaptiveCardFactory.GetTextCard(message);
                            }
                        }
                    }
                    break;

                case FlightFindingStates.GetDestination:
                    {
                        var userInput = turnContext.Activity.Text;
                        var destination = await _textExtractorService.ExtractDestination(userInput);
                        var airport = userProfile.SelectedAirport;

                        if (await _flightFindingService.CheckFlightsTo(airport, destination))
                        {
                            userProfile.Destination = destination;
                            conversationData.CurrentState = FlightFindingStates.GetFlightDate;

                            var message = MessageManager.DESTINATON_CONFIRMED(destination);
                            return AdaptiveCardFactory.GetCalanderCard(message);
                        }
                        else
                        {
                            conversationData.AirportsFound = null;
                            conversationData.CurrentState = FlightFindingStates.FindAirport;

                            var message = MessageManager.DESTINATION_NOT_AVAILIBLE(airport, destination);

                            return AdaptiveCardFactory.GetTextCard(message);
                        }
                    }

                case FlightFindingStates.GetFlightDate:
                    {
                        string usersSelectedDate = turnContext.Activity.Value.ToString();
                        var userInput = usersSelectedDate.GetDatefromCalander();
                        var displayDate = userInput.ToShortDateString();

                        if (userInput > DateTime.Now)
                        {
                            if (await _flightFindingService.CheckFlightsToOn(userProfile.SelectedAirport, userProfile.Destination, displayDate))
                            {
                                userProfile.FlightDate = userInput;
                                userProfile.DisplayFlightDate = displayDate;

                                string message = MessageManager.RETURN_FLIGHT_ASK(userProfile.Destination, displayDate);

                                conversationData.CurrentState = FlightFindingStates.GetReturnFlight;

                                return AdaptiveCardFactory.GetOptionalCalanderCard(message);
                            }
                            else
                            {
                                string message = MessageManager.NO_FLIGHTS_FOUND(userProfile.SelectedAirport, 
                                    userProfile.Destination, displayDate);

                                return AdaptiveCardFactory.GetCalanderCard(message);
                            }
                        }
                        else
                        {
                            string message = MessageManager.RECONFIRM_DATE(userProfile.Destination, displayDate);

                            return AdaptiveCardFactory.GetCalanderCard(message);
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

                                return AdaptiveCardFactory.GetCalanderCard(message);
                            }
                            else
                            {
                                var foundFlights = await _flightFindingService.FindFlights(
                                    userProfile.SelectedAirport, userProfile.Destination,
                                    userProfile.FlightDate, returnDate);

                                if (foundFlights.Count == 0)
                                {
                                    string message = MessageManager.RECONFIRM_DATE(userProfile.Destination, displayDate);

                                    return AdaptiveCardFactory.GetCalanderCard(message);
                                }
                                else
                                {
                                    conversationData.CurrentState = FlightFindingStates.FindAirport;

                                    var message = MessageManager.FOUND_RETURN_FLIGHTS(userProfile.SelectedAirport, 
                                        userProfile.Destination, userProfile.DisplayFlightDate, displayDate);

                                    return AdaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                                }
                            }
                        }
                        else
                        {
                            conversationData.CurrentState = FlightFindingStates.FindAirport;

                            var foundFlights = await _flightFindingService.FindFlights(
                                userProfile.SelectedAirport, userProfile.Destination,
                                userProfile.FlightDate);

                            var message = MessageManager.FOUND_FLIGHTS(userProfile.SelectedAirport, 
                                userProfile.Destination, userProfile.DisplayFlightDate);

                            return AdaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                        }
                    }
            }

            return null;
        }

        private async Task<Attachment> FindClosestAirports(ConversationData conversationData, bool isRestarting = false)
        {
            var airports = await _airportFindingService.FindClosestAirport();
            conversationData.AirportsFound = airports;

            var message = isRestarting? MessageManager.RESTART_MESSAGE(airports.Count) : 
                MessageManager.WELCOME_MESSAGE(airports.Count);

            return airports.Count==0? AdaptiveCardFactory.GetTextCard(message) :
                AdaptiveCardFactory.GetCaroselCard(message, airports);
        }
    }
}
