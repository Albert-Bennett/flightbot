using FlightBot.Services.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IFlightFindingService
    {
        Task<bool> CheckFlightsTo(ICollection<LocationData> airport, ICollection<LocationData> destination);

        Task<bool> CheckFlightsToOn(ICollection<LocationData> airport, ICollection<LocationData> destination, DateTime flightDate);

        Task<ICollection<string>> FindFlights(ICollection<LocationData> airport, ICollection<LocationData> destination, DateTime flightDate);

        Task<ICollection<string>> FindFlights(ICollection<LocationData> airport, ICollection<LocationData> destination, DateTime flightDate, DateTime returnDate);
    }
}
