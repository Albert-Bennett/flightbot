namespace FlightBot.Services.DataModels
{
    public class IATASearchAirportNames
    {
        public IATAAirportSearch[] SearchResults { get; set; }
    }

    public class IATAAirportSearch
    {
        public string IATACode { get; set; }
        public string AirportName { get; set; }
    }
}
