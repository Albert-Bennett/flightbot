using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBot.Services.DataModels
{
    public class AmadeusFlightSearchResult
    {
        public AmadeusFlightData[] data { get; set; }
    }

    public class AmadeusFlightData
    {
        public string type { get; set; }
        public string id { get; set; }
        public string source { get; set; }
        public bool instantTicketingRequired { get; set; }
        public bool nonHomogeneous { get; set; }
        public bool oneWay { get; set; }
        public string lastTicketingDate { get; set; }
        public int numberOfBookableSeats { get; set; }
        public Itinerary[] itineraries { get; set; }
        public Price price { get; set; }
        public Pricingoptions pricingOptions { get; set; }
        public string[] validatingAirlineCodes { get; set; }
        public Travelerpricing[] travelerPricings { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public string total { get; set; }
        public string _base { get; set; }
        public Fee[] fees { get; set; }
        public string grandTotal { get; set; }
    }

    public class Fee
    {
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class Pricingoptions
    {
        public string[] fareType { get; set; }
        public bool includedCheckedBagsOnly { get; set; }
    }

    public class Itinerary
    {
        public string duration { get; set; }
        public Segment[] segments { get; set; }
    }

    public class Segment
    {
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public string carrierCode { get; set; }
        public string number { get; set; }
        public Aircraft1 aircraft { get; set; }
        public Operating operating { get; set; }
        public string duration { get; set; }
        public string id { get; set; }
        public int numberOfStops { get; set; }
        public bool blacklistedInEU { get; set; }
    }

    public class Departure
    {
        public string iataCode { get; set; }
        public string terminal { get; set; }
        public DateTime at { get; set; }
    }

    public class Arrival
    {
        public string iataCode { get; set; }
        public string terminal { get; set; }
        public DateTime at { get; set; }
    }

    public class Aircraft1
    {
        public string code { get; set; }
    }

    public class Operating
    {
        public string carrierCode { get; set; }
    }

    public class Travelerpricing
    {
        public string travelerId { get; set; }
        public string fareOption { get; set; }
        public string travelerType { get; set; }
        public Price1 price { get; set; }
        public Faredetailsbysegment[] fareDetailsBySegment { get; set; }
    }

    public class Price1
    {
        public string currency { get; set; }
        public string total { get; set; }
        public string _base { get; set; }
    }

    public class Faredetailsbysegment
    {
        public string segmentId { get; set; }
        public string cabin { get; set; }
        public string fareBasis { get; set; }
        public string _class { get; set; }
        public Includedcheckedbags includedCheckedBags { get; set; }
    }

    public class Includedcheckedbags
    {
        public int weight { get; set; }
        public string weightUnit { get; set; }
    }
}
