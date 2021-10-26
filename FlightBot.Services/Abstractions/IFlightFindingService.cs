using FlightBot.Conversation;
using FlightBot.Services.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IFlightFindingService
    {
        Task<FlightCardData> FindFlights(ICollection<LocationData> origins, ICollection<LocationData> destinations, DateTime flightDate, DateTime? returnDate);
    }
}
