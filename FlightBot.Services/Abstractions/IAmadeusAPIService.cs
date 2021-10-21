using FlightBot.Services.DataModels;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IAmadeusAPIService
    {
        Task<AmadeusAirportSearch> SearchForAirports(string airport);
        Task<AmadeusNearbyAirportSearch> SearchForNearbyAirports(double latitude, double longitude);
    }
}
