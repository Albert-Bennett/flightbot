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
    }
}
