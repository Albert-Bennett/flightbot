using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FlightBot.Services
{
    public class AmadeusAPIService : BaseAPIService, IAmadeusAPIService
    {
        readonly HttpClient httpTokenClient;
        readonly HttpContent httpTokenContent;

        readonly int expiryModifier = 300; //represents 5 minutes in seconds

        DateTime expiry;
        string token;

        public AmadeusAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory):
            base(httpClientFactory, configuration["amadeus_base_url"])
        {
            httpTokenClient = httpClientFactory.CreateClient("AmadeusToken");
            httpTokenClient.BaseAddress = new Uri(configuration["amadeus_token_url"]);

            httpTokenContent = new FormUrlEncodedContent( new Dictionary<string, string>
            {
                { "client_id", configuration["amadeus_client_id"] },
                { "client_secret", configuration["amadeus_client_secret"] },
                { "grant_type", "client_credentials" }
            });
        }

        //public async Task<AmadeusNearbyAirportSearch> SearchForNearbyAirports(double latitude, double longitude)
        //{
        //    string endpoint = $"reference-data/locations/airports?latitude={latitude}&longitude={longitude}&radius={searchRadius}&sort=distance";

        //    return await GetAsync<AmadeusNearbyAirportSearch>(endpoint, await GetTokenAsync());
        //}

        //public async Task<AmadeusAirportSearch> SearchForAirports(string airport)
        //{
        //    string endpoint = $"reference-data/locations?subType=AIRPORT&keyword={airport}";

        //    return await GetAsync<AmadeusAirportSearch>(endpoint, await GetTokenAsync());
        //}

        public async Task<AmadeusFlightData[]> FindFlightAsync(string originIATACode, string destinationIATACode, DateTime flightDate, DateTime? returnDate)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["originLocationCode"] = originIATACode;
            query["destinationLocationCode"] = destinationIATACode;
            query["departureDate"] = flightDate.ToString("yyyy-MM-dd");

            if (returnDate !=null)
            {
                query["returnDate"] = returnDate.Value.ToString("yyyy-MM-dd");
            }

            query["adults"] = "1";
            query["currencyCode"] = "EUR";
            query["max"] = "2";
            query["travelClass"] = "ECONOMY";

            var offers = await GetAsync<AmadeusFlightSearchResult>($"shopping/flight-offers?{query}", await GetTokenAsync());
            return offers.data.OrderBy(x => x.price.grandTotal).ToArray();
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
