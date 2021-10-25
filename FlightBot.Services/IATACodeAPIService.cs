using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class IATACodeAPIService : BaseAPIService, IIATACodeAPIService
    {
        public IATACodeAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory) :
            base(httpClientFactory, configuration["IATA_Code_API_Endpoint"]) { }

        public async Task<IATASearchResponse> SerachForIATACodes(string searchTerm)
        {
            return await GetAsync<IATASearchResponse>("");
        }
    }
}
