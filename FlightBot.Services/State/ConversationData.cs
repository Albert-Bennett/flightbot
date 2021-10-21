using System.Collections.Generic;

namespace FlightBot.Services.State
{
    public class ConversationData
    {
        public FlightFindingStates CurrentState { get; set; } = FlightFindingStates.FindAirport;

        //Airports initially found
        public List<string> AirportsFound { get; set; }
    }
}
