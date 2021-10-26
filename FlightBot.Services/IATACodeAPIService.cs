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
        readonly string searchAirportFunctionCode;

        readonly string iataCodeEndpoint;
        readonly string airportSearchEndpoint;

        public IATACodeAPIService(IConfiguration configuration, IHttpClientFactory httpClientFactory) :
            base(httpClientFactory, configuration["IATA_Code_API_Endpoint"])
        {
            searchForIATACodeFunctionCode = configuration["IATA_API_IATAFunctionCode"];
            searchAirportFunctionCode = configuration["IATA_API_AirportFunctionCode"];

            iataCodeEndpoint = configuration["IATA_API_IATAEndpoint"];
            airportSearchEndpoint = configuration["IATA_API_AirportEndpoint"];
        }

        public async Task<IATASearchAirportNames> SearchAirportNames(string[] iataCodes)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = searchAirportFunctionCode;
            query["iataCodes"] = string.Join(",", iataCodes);

            return await GetAsync<IATASearchAirportNames>($"{airportSearchEndpoint}?{query}");
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
