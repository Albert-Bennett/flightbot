using System.Collections.Generic;

namespace FlightBot.Services.State
{
    public class ConversationData
    {
        public FlightFindingStates CurrentState { get; set; } = FlightFindingStates.FindAirport;

        public ICollection<string> NearbyAirports { get; set; }
        public ICollection<string> DestinationAirports { get; set; }
    }
}
