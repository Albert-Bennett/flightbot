namespace FlightBot.Services.DataModels
{
    public class GeonamesSearchResult
    {
        public Geoname[] geonames { get; set; }
    }

    public class Geoname
    {
        public string toponymName { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }
        public string adminName1 { get; set; }
        public string geonameId { get; set; }
    }
}
