using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace FlightBot.Conversation.Factories.Abstractions
{
    public interface IAdaptiveCardFactory
    {
        Attachment GetFoundFlightsCard(FlightCardData flightCardData);
        Attachment GetOptionalCalanderCard(string message);
        Attachment GetCalanderCard(string message);
        Attachment GetTextCard(string message);
        Attachment GetActionCard(string message, string action);
        Attachment GetCaroselCard(string message, ICollection<string> actions);
    }
}
