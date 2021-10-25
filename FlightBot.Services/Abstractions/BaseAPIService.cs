using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public abstract class BaseAPIService
    {
        readonly HttpClient httpClient;

        public BaseAPIService(IHttpClientFactory httpClientFactory, string baseUri)
        {
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUri);
        }

        protected async Task<T> GetAsync<T>(string endpoint, string token = "") where T : new()
        {
            if (token != string.Empty)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            throw new HttpRequestException($"Failed to get data from  {httpClient.BaseAddress}: {endpoint}. Status code: {response.StatusCode}");
        }
    }
}
