using FlightBot.Services.DataModels;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IGeonamesAPIService
    {
        Task<GeonamesSearchResult> SearchForNearbyAirports(double latitude, double longitude);
        Task<GeonamesSearchResult> SearchForAirports(string airport);
    }
}
