using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class GeonamesAPIService : IGeonamesAPIService
    {
        readonly HttpClient httpClient;
        readonly string searchRadius;
        readonly string geonamesUsername;

        public GeonamesAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("GeonamesAPI");
            httpClient.BaseAddress = new Uri(configuration["geonames_base_url"]);

            searchRadius = configuration["search_radius"];
            geonamesUsername = configuration["geonames_username"];
        }

        public async Task<GeonamesNearbyAirportSearch> SearchForNearbyAirports(double latitude, double longitude)
        {
            string endpoint = $"findNearbyJSON?lat={latitude}&lng={longitude}&fcode=AIRP&radius={searchRadius}&maxRows=10&username={geonamesUsername}";

            return await GetFromGeonamesAPI<GeonamesNearbyAirportSearch>(endpoint);
        }

        public async Task<GeonamesAirportSearch> SearchForAirports(string airport) 
        {
            string endpoint = $"searchJSON?maxRows=10&q={airport}&username={geonamesUsername}&fcode=AIRP";

            return await GetFromGeonamesAPI<GeonamesAirportSearch>(endpoint);
        }

        async Task<T> GetFromGeonamesAPI<T>(string endpoint) where T : new()
        {
            var response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            throw new HttpRequestException($"Failed to get data from  the Geonames API: {endpoint}. Status code: {response.StatusCode}");
        }
    }
}
