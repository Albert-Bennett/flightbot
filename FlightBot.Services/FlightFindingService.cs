using FlightBot.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class FlightFindingService : IFlightFindingService
    {
        public async Task<bool> CheckFlightsTo(ICollection<string> airport, ICollection<string> destination)
        {
            return true;
        }

        public async Task<bool> CheckFlightsToOn(ICollection<string> airport, ICollection<string> destination, DateTime flightDate)
        {
            //var userInput = DateTime.Parse(flightDate);

            //return airport.Equals("DUBLIN") && destination.Equals("Canada") && userInput > DateTime.Now;

            return true;
        }

        public async Task<ICollection<string>> FindFlights(ICollection<string> airport, ICollection<string> destination, DateTime flightDate)
        {
            // return airport.Equals("DUBLIN") && destination.Equals("Canada") && flightDate > DateTime.Now && returnDate > flightDate ?
            //      new List<string>() { "https://google.ie" } : new List<string>();

            return new List<string>() { "https://google.ie" };
        }

        public async Task<ICollection<string>> FindFlights(ICollection<string> airport, ICollection<string> destination, DateTime flightDate, DateTime returnDate)
        {
            //return airport.Equals("DUBLIN") && destination.Equals("Canada") && flightDate > DateTime.Now ?
            //    new List<string>() { "https://google.ie" } : new List<string>();

            return new List<string>() { "https://google.ie" };
        }
    }
}
