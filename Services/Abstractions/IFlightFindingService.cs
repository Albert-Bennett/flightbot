using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IFlightFindingService
    {
        Task<bool> CheckFlightsTo(string airport, string destination);

        Task<bool> CheckFlightsToOn(string airport, string destination, string flightDate);

        Task<ICollection<string>> FindFlights(string airport, string destination, DateTime flightDate);

        Task<ICollection<string>> FindFlights(string airport, string destination, DateTime flightDate, DateTime returnDate);
    }
}
