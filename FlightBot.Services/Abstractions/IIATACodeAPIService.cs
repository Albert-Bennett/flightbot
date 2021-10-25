using FlightBot.Services.DataModels;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IIATACodeAPIService
    {
        Task<IATASearchResponse> SerachForIATACodes(string searchTerm);
    }
}
