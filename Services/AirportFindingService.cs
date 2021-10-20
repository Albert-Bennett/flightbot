﻿using FlightBot.Bots.State.Helpers;
using FlightBot.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class AirportFindingService : IAirportFindingService
    {
        public async Task<List<string>> FindClosestAirport()
        {
            return new List<string>() { "DUBLIN", "NOT DUBLIN" };
        }

        public async Task<bool> ConfirmAirportExists(string airport)
        {
            return airport.Equals("DUBLIN");
        }

        public async Task<bool> CheckFlightsTo(string airport, string destination)
        {
            return airport.Equals("DUBLIN") && destination.Equals("Canada");
        }

        public async Task<bool> CheckFlightsToOn(string airport, string destination, string flightDate)
        {
            var userInput = DateTime.Parse(flightDate);

            return airport.Equals("DUBLIN") && destination.Equals("Canada") && userInput > DateTime.Now;
        }

        public async Task<ICollection<string>> FindFlights(string airport, string destination, DateTime flightDate)
        {
            return airport.Equals("DUBLIN") && destination.Equals("Canada") && flightDate > DateTime.Now ?
                new List<string>() { "https://google.ie" } : new List<string>();
        }

        public async Task<ICollection<string>> FindFlights(string airport, string destination, DateTime flightDate, DateTime returnDate)
        {
            return airport.Equals("DUBLIN") && destination.Equals("Canada") && flightDate > DateTime.Now && returnDate > flightDate ?
                new List<string>() { "https://google.ie" } : new List<string>();
        }
    }
}
