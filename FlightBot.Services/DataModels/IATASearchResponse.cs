namespace FlightBot.Services.DataModels
{
    public class IATASearchResponse
    {
        public IATACodeEntity[] SearchResults { get; set; }
    }

    public class IATACodeEntity
    {
        public string IATACode { get; set; }
        public string CityAirport { get; set; }
        public string Country { get; set; }
        public string geonameId { get; set; }
    }
}
