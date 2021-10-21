namespace FlightBot.Conversation
{
    public class MessageManager
    {
        public static string NO_SUITABLE_FLIGHTS => Messages.NO_SUITABLE_FLIGHTS;
        public static string AIRPORT_CONFIRMED => Messages.AIRPORT_CONFIRMED;
        public static string AIRPORT_NOT_FOUND => Messages.AIRPORT_NOT_FOUND;
        public static string DESTINATON_CONFIRMED(string destination) =>
            Messages.DESTINATON_CONFIRMED.Replace(ReplaceTokens.Destination, destination);
        public static string DESTINATION_NOT_AVAILIBLE(string airport, string destination)=>
            Messages.DESTINATION_NOT_AVAILIBLE.
                Replace(ReplaceTokens.Airport, airport).
                Replace(ReplaceTokens.Destination, destination);
        public static string RETURN_FLIGHT_ASK (string destination, string flightDate)=>
            Messages.RETURN_FLIGHT_ASK.
                Replace(ReplaceTokens.Destination, destination).
                Replace(ReplaceTokens.FlightDate, flightDate);
        public static string NO_FLIGHTS_FOUND (string airport, string destination, string flightDate)=>
            Messages.NO_FLIGHTS_FOUND.
                Replace(ReplaceTokens.Airport, airport).
                Replace(ReplaceTokens.Destination, destination).
                Replace(ReplaceTokens.Date, flightDate);
        public static string RECONFIRM_DATE(string destination, string flightDate)=>
            Messages.RECONFIRM_DATE.
                Replace(ReplaceTokens.Destination, destination).
                Replace(ReplaceTokens.FlightDate, flightDate);
        public static string INVALID_RETURN_DATE(string flightDate, string returnDate)=>
            Messages.INVALID_RETURN_DATE.
                Replace(ReplaceTokens.ReturnDate, returnDate).
                Replace(ReplaceTokens.FlightDate, flightDate);
        public static string FOUND_RETURN_FLIGHTS(string airport, string destination, string flightDate, string returnDate)=>
            Messages.FOUND_RETURN_FLIGHTS.
                Replace(ReplaceTokens.Destination, destination).
                Replace(ReplaceTokens.FlightDate, flightDate).
                Replace(ReplaceTokens.Airport, airport).
                Replace(ReplaceTokens.ReturnDate, returnDate);
        public static string FOUND_FLIGHTS(string airport, string destination, string flightDate)=>
        Messages.FOUND_FLIGHTS.
            Replace(ReplaceTokens.Destination, destination).
            Replace(ReplaceTokens.FlightDate, flightDate).
            Replace(ReplaceTokens.Airport, airport);
        public static string WELCOME_MESSAGE(int airportsFound) =>
            airportsFound == 0 ? Messages.WELCOME_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_NO_AIRPORTS) :
            airportsFound == 1 ? Messages.WELCOME_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_ONE_AIRPORT) :
            Messages.WELCOME_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_MANY_AIRPORTS);
        public static string RESTART_MESSAGE(int airportsFound)=>
            airportsFound == 0 ? Messages.RESTART_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_NO_AIRPORTS) :
            airportsFound == 1 ? Messages.RESTART_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_ONE_AIRPORT) :
            Messages.RESTART_MESSAGE.Replace(ReplaceTokens.AirportResponse, Messages.FOUND_MANY_AIRPORTS);
    }
}
