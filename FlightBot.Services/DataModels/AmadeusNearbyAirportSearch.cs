namespace FlightBot.Services.DataModels
{
    public class AmadeusNearbyAirportSearch
    {
        public Meta meta { get; set; }
        public AmadeusNearbyAirportSearchDatum[] data { get; set; }
    }

    public class AmadeusNearbyAirportSearchDatum
    {
        public string type { get; set; }
        public string subType { get; set; }
        public string name { get; set; }
        public string detailedName { get; set; }
        public string timeZoneOffset { get; set; }
        public string iataCode { get; set; }
        public Geocode geoCode { get; set; }
        public AmadeusNearbyAirportSearchAddress address { get; set; }
        public Distance distance { get; set; }
        public Analytics analytics { get; set; }
        public float relevance { get; set; }
    }

    public class AmadeusNearbyAirportSearchAddress
    {
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string regionCode { get; set; }
    }

    public class Distance
    {
        public int value { get; set; }
        public string unit { get; set; }
    }

    public class Analytics
    {
        public Flights flights { get; set; }
        public Travelers travelers { get; set; }
    }

    public class Flights
    {
        public int score { get; set; }
    }

    public class Travelers
    {
        public int score { get; set; }
    }

}
