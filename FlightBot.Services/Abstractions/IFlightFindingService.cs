using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IFlightFindingService
    {
        Task<bool> CheckFlightsTo(ICollection<string> airport, ICollection<string> destination);

        Task<bool> CheckFlightsToOn(ICollection<string> airport, ICollection<string> destination, DateTime flightDate);

        Task<ICollection<string>> FindFlights(ICollection<string> airport, ICollection<string> destination, DateTime flightDate);

        Task<ICollection<string>> FindFlights(ICollection<string> airport, ICollection<string> destination, DateTime flightDate, DateTime returnDate);
    }
}
