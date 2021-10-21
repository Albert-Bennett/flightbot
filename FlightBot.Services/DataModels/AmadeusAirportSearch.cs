namespace FlightBot.Services.DataModels
{
    public class AmadeusAirportSearch
    {
        public Meta meta { get; set; }
        public AmadeusAirportSearchDatum[] data { get; set; }
    }

    public class Meta
    {
        public int count { get; set; }
        public Links links { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class AmadeusAirportSearchDatum
    {
        public string type { get; set; }
        public string subType { get; set; }
        public string name { get; set; }
        public string detailedName { get; set; }
        public string id { get; set; }
        public Self self { get; set; }
        public string timeZoneOffset { get; set; }
        public string iataCode { get; set; }
        public Geocode geoCode { get; set; }
        public AmadeusAirportSearchAddress address { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
        public string[] methods { get; set; }
    }

    public class Geocode
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
    }

    public class AmadeusAirportSearchAddress
    {
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string stateCode { get; set; }
        public string regionCode { get; set; }
    }

}
