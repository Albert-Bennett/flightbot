namespace FlightBot.Services.DataModels
{
    public class GeonamesNearbyAirportSearch
    {
        public GeonamesNearbyAirportSearchGeoname[] geonames { get; set; }
    }

    public class GeonamesNearbyAirportSearchGeoname
    {
        public string toponymName { get; set; }
    }
}
