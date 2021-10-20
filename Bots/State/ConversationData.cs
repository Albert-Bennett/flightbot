using System.Collections.Generic;

namespace FlightBot.Bots.State
{
    public enum FlightFindingStates : byte
    {
        FindAirport = 0,
        GetFlightDate = 1,
        GetDestination = 2,
        GetReturnFlight = 3
    }

    public class ConversationData
    {
        public FlightFindingStates CurrentState { get; set; } = FlightFindingStates.FindAirport;

        //Airports initially found
        public List<string> AirportsFound { get; set; }

        //Airports checked for flights to destination used when flight not availible to destination
        public List<string> AirportsChecked { get; set; }
    }
}
