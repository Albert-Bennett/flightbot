using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class AirportFindingService : IAirportFindingService
    {
        readonly IGeonamesAPIService _geonamesAPIService;

        public AirportFindingService(IGeonamesAPIService geonamesAPIService)
        {
            _geonamesAPIService = geonamesAPIService;
        }

        public async Task<ICollection<string>> FindClosestAirports()
        {
            //TODO: find a better way of getting the users IP other that using a dummy IP address 
            double latitude = 53.429174;
            double longitude = -6.238352;

            var searchResult = await _geonamesAPIService.SearchForNearbyAirports(latitude, longitude);
            return ProcessAirpotSearchResults(searchResult.geonames);
        }

        public async Task<ICollection<string>> FindAssociatedAirports(string airport)
        {
            var searchResult = await _geonamesAPIService.SearchForAirports(airport);

            return ProcessAirpotSearchResults(searchResult.geonames);
        }

        private static List<string> ProcessAirpotSearchResults(Geoname[] searchResult)
        {
            List<string> airports = new();

            foreach (var airport in searchResult)
            {
                if (airport.toponymName.Contains("Airport"))
                {
                    airports.Add(airport.toponymName);
                }
            }

            return airports;
        }
    }
}
