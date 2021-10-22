using FlightBot.Services.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IAirportFindingService
    {
        Task<ICollection<LocationData>> FindClosestAirports();
        
        Task<ICollection<LocationData>> FindAssociatedAirports(string airport);
    }
}
