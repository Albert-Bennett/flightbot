using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FlightBot.Services
{
    public class IATACodeAPIService : BaseAPIService, IIATACodeAPIService
    {
        readonly string searchForIATACodeFunctionCode;
        readonly string iataCodeEndpoint;

        public IATACodeAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory) :
            base(httpClientFactory, configuration["IATA_Code_API_Endpoint"])
        {
            searchForIATACodeFunctionCode = configuration["IATA_API_IATAFunctionCode"];
            iataCodeEndpoint = configuration["IATA_API_IATAEndpoint"];
        }

        public async Task<IATASearchResponse> SearchForIATACodes(string searchTerm, string geonameId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = searchForIATACodeFunctionCode;
            query["airport"] = searchTerm;
            query["geonameId"] = geonameId;

            return await GetAsync<IATASearchResponse>($"{iataCodeEndpoint}?{query}");
        }        
    }
}
