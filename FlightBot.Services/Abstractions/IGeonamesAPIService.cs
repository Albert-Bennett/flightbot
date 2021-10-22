using FlightBot.Services.DataModels;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IGeonamesAPIService
    {
        Task<GeonamesNearbyAirportSearch> SearchForNearbyAirports(double latitude, double longitude);
        Task<GeonamesAirportSearch> SearchForAirports(string airport);
    }
}
