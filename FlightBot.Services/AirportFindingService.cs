using FlightBot.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class AirportFindingService : IAirportFindingService
    {
        readonly IAmadeusAPIService _amadeusAPIService;

        public AirportFindingService(IAmadeusAPIService amadeusAPIService)
        {
            _amadeusAPIService = amadeusAPIService;
        }

        public async Task<List<string>> FindClosestAirport()
        {
            double latitude = 53.429174;
            double longitude = -6.238352;

            var searchResult = await _amadeusAPIService.SearchForNearbyAirports(latitude, longitude);

            List<string> airports = new();

            foreach(var airport in searchResult.data)
            {
                airports.Add(airport.detailedName);
            }

            return airports;
        }

        public async Task<bool> ConfirmAirportExists(string airport)
        {
            var searchResult = await _amadeusAPIService.SearchForAirports(airport);

            return searchResult.data.Length > 0;
        }
    }
}
