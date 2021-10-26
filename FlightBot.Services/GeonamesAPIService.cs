using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class GeonamesAPIService : BaseAPIService, IGeonamesAPIService
    {
        readonly string searchRadius;
        readonly string geonamesUsername;

        public GeonamesAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory):
            base(httpClientFactory, configuration["geonames_base_url"])
        {
            searchRadius = configuration["search_radius"];
            geonamesUsername = configuration["geonames_username"];
        }

        public async Task<GeonamesSearchResult> SearchForNearbyAirports(double latitude, double longitude)
        {
            string endpoint = $"findNearbyJSON?lat={latitude}&lng={longitude}&fcode=AIRP&radius={searchRadius}&maxRows=10&username={geonamesUsername}";

            return await GetAsync<GeonamesSearchResult>(endpoint);
        }

        public async Task<GeonamesSearchResult> SearchForAirports(string airport) 
        {
            string endpoint = $"searchJSON?maxRows=10&q={airport}&username={geonamesUsername}&fcode=AIRP";

            return await GetAsync<GeonamesSearchResult>(endpoint);
        }
    }
}
