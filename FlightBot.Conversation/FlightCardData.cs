using System;
using System.Collections.Generic;

namespace FlightBot.Conversation
{
    public class FlightCardData
    {
        public string Airport { get; set; }
        public string Destination { get; set; }
        public string AirportIATACode { get; set; }
        public string DestinationIATACode { get; set; }
        public DateTime DepartureDate { get; set; }
        public int SeatsAvailible { get; set; }
        public string Currency { get; set; }
        public string MaxPrice { get; set; }

        public List<StopDetails> StopDetails { get; set; }
    }

    public class StopDetails
    {
        public string Duration { get; set; }

        public List<StopSegment> Segments { get; set; }
    }

    public class StopSegment
    {
        public DateTime ArivialDate;
        public string ArivialTerminal;
        public string ArivialIATACode;
        public DateTime DepartureDate;
        public string DepartureTerminal;
        public string DepartureIATACode;
        public string Duration;
    }
}
