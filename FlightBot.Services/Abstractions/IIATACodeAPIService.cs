using FlightBot.Services.DataModels;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IIATACodeAPIService
    {
        Task<IATASearchResponse> SearchForIATACodes(string searchTerm, string geonameId);
        Task<IATASearchAirportNames> SearchAirportNames(string[] iataCodes);
    }
}
