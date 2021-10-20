using System;

namespace FlightBot.Bots.State
{
    public class UserProfile
    {
        public string SelectedAirport { get; set; }
        public string Destination { get; set; }
        public DateTime FlightDate { get; set; }

        public string DisplayFlightDate { get; set; }
    }
}
