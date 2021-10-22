using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IAirportFindingService
    {
        Task<ICollection<string>> FindClosestAirports();
        
        Task<ICollection<string>> FindAssociatedAirports(string airport);
    }
}
