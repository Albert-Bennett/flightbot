using FlightBot.Bots.State;
using FlightBot.Bots.State.Helpers;
using FlightBot.Services.Abstractions;
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

        public StateManagerService(IAirportFindingService airportFindingService,
            ITextExtractorService textExtractorService)
        {
            _airportFindingService = airportFindingService;
            _textExtractorService = textExtractorService;
        }

        public async Task<Attachment> GenerateCurentState(UserProfile userProfile,
            ConversationData conversationData, ITurnContext turnContext)
        {
            switch (conversationData.CurrentState)
            {
                case FlightFindingStates.FindAirport:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (conversationData.AirportsFound == null || userInput.Equals(Messages.NO_SUITABLE_FLIGHTS))
                        {
                            return await FindClosestAirports(conversationData);
                        }
                        else if (conversationData.AirportsFound != null && !string.IsNullOrEmpty(userInput))
                        {
                            if(await _airportFindingService.ConfirmAirportExists(userInput)) 
                            {
                                userProfile.SelectedAirport = userInput;

                                conversationData.CurrentState = FlightFindingStates.GetDestination;

                                var message = Messages.AIRPORT_CONFIRMED;
                                return AdaptiveCardFactory.GetTextCard(message);
                            }
                            else 
                            {
                                var message = Messages.AIRPORT_NOT_FOUND;
                                return AdaptiveCardFactory.GetTextCard(message);
                            }
                        }
                    }
                    break;

                case FlightFindingStates.GetDestination:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (string.IsNullOrEmpty(userProfile.Destination) && !string.IsNullOrEmpty(userInput))
                        {
                            var destination = await _textExtractorService.ExtractDestination(userInput);
                            var airport = userProfile.SelectedAirport;

                            if(await _airportFindingService.CheckFlightsTo(airport, destination)) 
                            {
                                userProfile.Destination = destination;
                                conversationData.CurrentState = FlightFindingStates.GetFlightDate;

                                var message = Messages.DESTINATON_CONFIRMED.Replace(ReplaceTokens.Destination, destination);
                                return AdaptiveCardFactory.GetCalanderCard(message);
                            }
                            else
                            {
                                conversationData.AirportsFound = null;
                                conversationData.CurrentState = FlightFindingStates.FindAirport;

                                var message = Messages.DESTINATION_NOT_AVAILIBLE.
                                    Replace(ReplaceTokens.Airport, airport).
                                    Replace(ReplaceTokens.Destination, destination);

                                return AdaptiveCardFactory.GetTextCard(message);
                            }
                        }
                    }
                    break;

                case FlightFindingStates.GetFlightDate:
                    {
                        string usersSelectedDate = turnContext.Activity.Value.ToString();
                        var userInput = AdaptiveCardDateParser.GetDatefromUserInput(usersSelectedDate);
                        var displayDate = userInput.ToShortDateString();

                        if (userInput > DateTime.Now)
                        {
                            if (await _airportFindingService.CheckFlightsToOn(userProfile.SelectedAirport, userProfile.Destination, displayDate))
                            {
                                userProfile.FlightDate = userInput;
                                userProfile.DisplayFlightDate = displayDate;

                                string message = Messages.RETURN_FLIGHT_ASK.
                                    Replace(ReplaceTokens.Destination, userProfile.Destination).
                                    Replace(ReplaceTokens.FlightDate, displayDate);

                                conversationData.CurrentState = FlightFindingStates.GetReturnFlight;

                                return AdaptiveCardFactory.GetOptionalCalanderCard(message);
                            }
                            else
                            {
                                string message = Messages.NO_FLIGHTS_FOUND.
                                    Replace(ReplaceTokens.Airport, userProfile.SelectedAirport).
                                    Replace(ReplaceTokens.Destination, userProfile.Destination).
                                    Replace(ReplaceTokens.Date, displayDate);

                                return AdaptiveCardFactory.GetCalanderCard(message);
                            }
                        }
                        else
                        {
                            string destination = userProfile.Destination;
                            string message = Messages.RECONFIRM_DATE.
                                Replace(ReplaceTokens.Destination, destination).
                                Replace(ReplaceTokens.FlightDate, displayDate);

                            return AdaptiveCardFactory.GetCalanderCard(message);
                        }
                    }

                case FlightFindingStates.GetReturnFlight:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (userInput == null)
                        {
                            string usersSelectedDate = turnContext.Activity.Value.ToString();
                            var returnDate = AdaptiveCardDateParser.GetDatefromUserInput(usersSelectedDate);
                            var displayDate = returnDate.ToShortDateString();

                            if (userProfile.FlightDate >= returnDate)
                            {
                                string message = Messages.INVALID_RETURN_DATE.
                                    Replace(ReplaceTokens.ReturnDate, displayDate).
                                    Replace(ReplaceTokens.FlightDate, userProfile.DisplayFlightDate);

                                return AdaptiveCardFactory.GetCalanderCard(message);
                            }
                            else
                            {
                                var foundFlights = await _airportFindingService.FindFlights(
                                    userProfile.SelectedAirport, userProfile.Destination,
                                    userProfile.FlightDate, returnDate);

                                if (foundFlights.Count == 0)
                                {
                                    string message = Messages.RECONFIRM_DATE.
                                        Replace(ReplaceTokens.Destination, userProfile.Destination).
                                        Replace(ReplaceTokens.FlightDate, displayDate);

                                    return AdaptiveCardFactory.GetCalanderCard(message);
                                }
                                else
                                {
                                    conversationData.CurrentState = FlightFindingStates.FindAirport;

                                    var message = Messages.FOUND_RETURN_FLIGHTS.
                                        Replace(ReplaceTokens.Destination, userProfile.Destination).
                                        Replace(ReplaceTokens.FlightDate, userProfile.DisplayFlightDate).
                                        Replace(ReplaceTokens.Airport, userProfile.SelectedAirport).
                                        Replace(ReplaceTokens.ReturnDate, displayDate);

                                    return AdaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                                }
                            }
                        }
                        else
                        {
                            conversationData.CurrentState = FlightFindingStates.FindAirport;

                            var foundFlights = await _airportFindingService.FindFlights(
                                userProfile.SelectedAirport, userProfile.Destination,
                                userProfile.FlightDate);

                            var message = Messages.FOUND_FLIGHTS.
                                Replace(ReplaceTokens.Destination, userProfile.Destination).
                                Replace(ReplaceTokens.FlightDate, userProfile.DisplayFlightDate).
                                Replace(ReplaceTokens.Airport, userProfile.SelectedAirport);

                            return AdaptiveCardFactory.GetFoundFlightsCard(message, foundFlights);
                        }
                    }
            }

            return null;
        }

        private async Task<Attachment> FindClosestAirports(ConversationData conversationData)
        {
            var airports = await _airportFindingService.FindClosestAirport();
            conversationData.AirportsFound = airports;

            var welcomeMessage = Messages.WELCOME_MESSAGE;

            if (airports.Count == 0)
            {
                welcomeMessage = welcomeMessage.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_NO_AIRPORTS);
                return AdaptiveCardFactory.GetTextCard(welcomeMessage);
            }
            else if (airports.Count == 1)
            {
                welcomeMessage = welcomeMessage.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_ONE_AIRPORT);
                return AdaptiveCardFactory.GetCaroselCard(welcomeMessage, airports);
            }

            welcomeMessage = welcomeMessage.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_MANY_AIRPORTS);
            return AdaptiveCardFactory.GetCaroselCard(welcomeMessage, airports);
        }
    }
}
