using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System.Collections.Generic;
using System.Linq;
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
                    foreach (var result in iataCodes.SearchResults)
                    {
                        if (airports.Count == 0 || airports.Any(x => !x.IATACode.Equals(result.IATACode)))
                        {
                            airports.Add(new LocationData
                            {
                                AirportName = result.CityAirport,
                                IATACode = result.IATACode,
                                CityName = airport.adminName1,
                                Lat = airport.lat,
                                Lng = airport.lng
                            });
                        }
                    }
                }
            }

            return airports;
        }
    }
}
