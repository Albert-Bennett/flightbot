using FlightBot.Services.DataModels;
using System.Collections.Generic;

namespace FlightBot.Services.State
{
    public class ConversationData
    {
        public FlightFindingStates CurrentState { get; set; } = FlightFindingStates.FindAirport;

        public ICollection<LocationData> NearbyAirports { get; set; }
        public ICollection<LocationData> DestinationAirports { get; set; }
    }
}
