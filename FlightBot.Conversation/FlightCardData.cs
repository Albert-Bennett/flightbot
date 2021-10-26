using System;

namespace FlightBot.Conversation
{
    public class FlightCardData
    {
        public string Airport { get; set; }
        public string Destination { get; set; }
        public string AirportIATACode { get; set; }
        public string DestinationIATACode { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureGame { get; set; }
        public int SeatsAvailible { get; set; }
        public string Currency { get; set; }
        public string MaxPrice { get; set; }
    }
}
