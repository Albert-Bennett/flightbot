namespace FlightBot.Services.DataModels
{
    public record LocationData
    {
        public string IATACode { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
