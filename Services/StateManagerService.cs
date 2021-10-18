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
        IAirportFindingService _airportFindingService;
        ITextExtractorService _textExtractorService;

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

                        if (conversationData.AirportsFound == null)
                        {
                            return await FindClosestAirports(conversationData);
                        }
                        else if (conversationData.AirportsFound != null && !string.IsNullOrEmpty(userInput))
                        {
                            //Confirm user input contains an airport
                            userProfile.SelectedAirport = userInput;

                            conversationData.CurrentState = FlightFindingStates.GetDestination;

                            var message = Messages.AIRPORT_CONFIRMED;
                            return AdaptiveCardFactory.GetTextCard(message);
                        }
                    }
                    break;

                case FlightFindingStates.GetDestination:
                    {
                        var userInput = turnContext.Activity.Text;

                        if (string.IsNullOrEmpty(userProfile.Destination) && !string.IsNullOrEmpty(userInput))
                        {
                            var destination = await _textExtractorService.ExtractDestination(userInput);
                            userProfile.Destination = destination;
                            conversationData.CurrentState = FlightFindingStates.GetFlightDate;

                            var message = Messages.DESTINATON_CONFIRMED;

                            message = message.Replace(ReplaceTokens.Destination, destination);
                            return AdaptiveCardFactory.GetCalanderCard(message);
                        }
                    }
                    break;

                case FlightFindingStates.GetFlightDate:
                    {
                        var userInput = AdaptiveCardDateParser.GetDatefromUserInput(turnContext.Activity.Value.ToString());

                        if (userInput.Ticks > DateTime.Now.Ticks)
                        {

                        }
                    }
                    break;
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
