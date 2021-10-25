using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class FlightFindingService : IFlightFindingService
    {
        readonly IAmadeusAPIService _amadeusAPIService;
        public FlightFindingService(IAmadeusAPIService amadeusAPIService)
        {
            _amadeusAPIService = amadeusAPIService;
        }

        public async Task<ICollection<string>> FindFlights(ICollection<LocationData> origins, ICollection<LocationData> destinations, DateTime flightDate, DateTime? returnDate)
        {
            List<string> foundFlights = new();

            foreach (var origin in origins)
            {
                foreach(var dest in destinations) 
                {
                    var foundFlight = await _amadeusAPIService.FindFlightAsync(origin.IATACode, dest.IATACode, flightDate, returnDate);

                    if (foundFlight != null)
                    {
                        foundFlights.Add("Yup");
                    }
                }
            }

            return foundFlights;
        }
    }
}
