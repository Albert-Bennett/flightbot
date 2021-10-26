using FlightBot.Conversation;
using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class FlightFindingService : IFlightFindingService
    {
        readonly IAmadeusAPIService _amadeusAPIService;
        public FlightFindingService(IAmadeusAPIService amadeusAPIService)
        {
            _amadeusAPIService = amadeusAPIService;
        }

        public async Task<FlightCardData> FindFlights(ICollection<LocationData> origins, ICollection<LocationData> destinations, DateTime flightDate, DateTime? returnDate)
        {
            FlightCardData foundFlights = null;

            foreach (var origin in origins)
            {
                foreach(var dest in destinations) 
                {
                    var foundFlight = await _amadeusAPIService.FindFlightAsync(origin.IATACode, dest.IATACode, flightDate, returnDate);

                    if (foundFlight != null)
                    {
                        foreach (var flight in foundFlight)
                        {
                            foundFlights = new FlightCardData
                            {
                                Airport = origin.AirportName,
                                AirportIATACode = origin.IATACode,
                                Currency = flight.price.currency,
                                Destination = dest.AirportName,
                                DestinationIATACode = dest.IATACode,
                                MaxPrice = flight.price.grandTotal,
                                SeatsAvailible = flight.numberOfBookableSeats,
                                DepartureDate = DateTime.Now, //should come from the flight segments
                                DepartureGame = "1" //should come from the segments
                            };
                        }
                    }
                }
            }

            return foundFlights; // just returning the first one and not the accumulated data 
        }
    }
}
