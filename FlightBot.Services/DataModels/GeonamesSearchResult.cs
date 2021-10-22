namespace FlightBot.Services.DataModels
{
    public class GeonamesSearchResult
    {
        public Geoname[] geonames { get; set; }
    }

    public class Geoname
    {
        public string toponymName { get; set; }
    }
}
