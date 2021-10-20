namespace FlightBot.Services.State
{
    public enum FlightFindingStates : byte
    {
        FindAirport = 0,
        GetFlightDate = 1,
        GetDestination = 2,
        GetReturnFlight = 3,
        SendJourneyDetails = 4
    }
}
