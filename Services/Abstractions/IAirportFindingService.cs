using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IAirportFindingService
    {
        Task<List<string>> FindClosestAirport();

        Task<bool> ConfirmAirportExists(string airport);
    }
}
