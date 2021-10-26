using FlightBot.Services.DataModels;
using System;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IAmadeusAPIService
    {
        Task<AmadeusFlightData[]> FindFlightAsync(string originIATACode, string destinationIATACode, DateTime flightDate, DateTime? returnDate);
    }
}