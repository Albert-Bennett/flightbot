using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class AirportFindingService : IAirportFindingService
    {
        readonly IGeonamesAPIService _geonamesAPIService;
        readonly IIATACodeAPIService _iataCodeAPIService;

        public AirportFindingService(IGeonamesAPIService geonamesAPIService, IIATACodeAPIService iataCodeAPIService)
        {
            _geonamesAPIService = geonamesAPIService;
            _iataCodeAPIService = iataCodeAPIService;
        }

        public async Task<ICollection<LocationData>> FindClosestAirports()
        {
            //TODO: find a better way of getting the users IP other that using a dummy IP address 
            double latitude = 53.429174;
            double longitude = -6.238352;

            var searchResult = await _geonamesAPIService.SearchForNearbyAirports(latitude, longitude);
            return await ProcessAirpotSearchResults(searchResult.geonames);
        }

        public async Task<ICollection<LocationData>> FindAssociatedAirports(string airport)
        {
            var searchResult = await _geonamesAPIService.SearchForAirports(airport);

            return await ProcessAirpotSearchResults(searchResult.geonames);
        }

        private async Task<List<LocationData>> ProcessAirpotSearchResults(Geoname[] searchResult)
        {
            List<LocationData> airports = new();

            foreach (var airport in searchResult)
            {
                var iataCodes = await _iataCodeAPIService.SearchForIATACodes(airport.toponymName, airport.geonameId);

                if (iataCodes.SearchResults.Length > 0)
                {
                    airports.Add(new LocationData
                    {
                        AirportName = iataCodes.SearchResults[0].CityAirport,
                        IATACode = iataCodes.SearchResults[0].IATACode,
                        CityName = airport.adminName1,
                        Lat = airport.lat,
                        Lng = airport.lng
                    });
                }
            }

            return airports;
        }
    }
}
