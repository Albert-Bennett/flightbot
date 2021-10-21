using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class AmadeusAPIService : IAmadeusAPIService
    {
        readonly HttpClient httpTokenClient;
        readonly HttpClient httpClient;
        readonly HttpContent httpTokenContent;

        readonly int expiryModifier = 300; //represents 5 minutes in seconds
        readonly string searchRadius;

        DateTime expiry;
        string token;

        public AmadeusAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            httpTokenClient = httpClientFactory.CreateClient("AmadeusToken");
            httpTokenClient.BaseAddress = new Uri(configuration["amadeus_token_url"]);

            httpTokenContent = new FormUrlEncodedContent( new Dictionary<string, string>
            {
                { "client_id", configuration["amadeus_client_id"] },
                { "client_secret", configuration["amadeus_client_secret"] },
                { "grant_type", "client_credentials" }
            });

            httpClient = httpClientFactory.CreateClient("AmadeusAPI");
            httpClient.BaseAddress = new Uri(configuration["amadeus_base_url"]);

            searchRadius = configuration["amadeus_search_radius"];
        }

        public async Task<AmadeusNearbyAirportSearch> SearchForNearbyAirports(double latitude, double longitude)
        {
            string endpoint = $"reference-data/locations/airports?latitude={latitude}&longitude={longitude}&radius={searchRadius}&sort=distance";

            return await GetFromAmadeusAPI<AmadeusNearbyAirportSearch>(endpoint);
        }

        public async Task<AmadeusAirportSearch> SearchForAirports(string airport)
        {
            string endpoint = $"reference-data/locations?subType=AIRPORT&keyword={airport}";

            return await GetFromAmadeusAPI<AmadeusAirportSearch>(endpoint);
        }

        async Task<T> GetFromAmadeusAPI<T>(string endpoint) where T : new()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await GetTokenAsync());

            var response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            throw new HttpRequestException($"Failed to get data from  the Amadeus API: {endpoint}. Status code: {response.StatusCode}");
        }

        async Task<string> GetTokenAsync()
        {
            return DateTime.Now < expiry ? token :
                await GenerateNewToken();
        }

        async Task<string> GenerateNewToken() 
        {
            var response = await httpTokenClient.PostAsync("", httpTokenContent);

            if (response.IsSuccessStatusCode) 
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var amadeusTokenResponse = JsonConvert.DeserializeObject<AmadeusTokenResponse>(responseContent);

                expiry = DateTime.Now.AddSeconds(amadeusTokenResponse.expires_in - expiryModifier);
                token = amadeusTokenResponse.access_token;

                return token;
            }

            throw new HttpRequestException($"Failed to get a Amadeus token. Status code: {response.StatusCode}");
        }
    }
}
