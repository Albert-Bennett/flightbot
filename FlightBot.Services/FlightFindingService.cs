using FlightBot.Conversation;
using FlightBot.Services.Abstractions;
using FlightBot.Services.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace FlightBot.Services
{
    public class FlightFindingService : IFlightFindingService
    {
        readonly IAmadeusAPIService _amadeusAPIService;
        public FlightFindingService(IAmadeusAPIService amadeusAPIService)
        {
            _amadeusAPIService = amadeusAPIService;
        }

        public async Task<ICollection<FlightCardData>> FindFlights(ICollection<LocationData> origins, ICollection<LocationData> destinations, DateTime flightDate, DateTime? returnDate)
        {
            List<FlightCardData> foundFlights = new ();

            foreach (var origin in origins)
            {
                foreach(var dest in destinations) 
                {
                    var foundFlight = await _amadeusAPIService.FindFlightAsync(origin.IATACode, dest.IATACode, flightDate, returnDate);

                    if (foundFlight != null)
                    {
                        foreach (var flight in foundFlight)
                        {
                            List<StopDetails> stopDetails = new();

                            foreach (var itenerary in flight.itineraries)
                            {
                                List<StopSegment> segments = new();

                                foreach (var seg in itenerary.segments)
                                {
                                    segments.Add(new StopSegment
                                    {
                                        ArivialDate = seg.arrival.at,
                                        ArivialIATACode = seg.arrival.iataCode,
                                        ArivialTerminal = seg.arrival.terminal,
                                        DepartureDate = seg.departure.at,
                                        DepartureIATACode = seg.departure.iataCode,
                                        DepartureTerminal = seg.departure.terminal,
                                        Duration = XmlConvert.ToTimeSpan(seg.duration).ToString(@"hh\:mm")
                                    });
                                }

                                stopDetails.Add(new StopDetails
                                {
                                    Duration = XmlConvert.ToTimeSpan(itenerary.duration).ToString(@"hh\:mm"),
                                    Segments = segments
                                });
                            }

                            var flightDetails = new FlightCardData
                            {
                                Airport = origin.AirportName,
                                AirportIATACode = origin.IATACode,
                                Currency = flight.price.currency,
                                Destination = dest.AirportName,
                                DestinationIATACode = dest.IATACode,
                                MaxPrice = flight.price.grandTotal,
                                SeatsAvailible = flight.numberOfBookableSeats,
                                DepartureDate = flight.itineraries[0].segments[0].departure.at,
                                StopDetails = stopDetails
                            };

                            foundFlights.Add(flightDetails);
                        }
                    }
                }
            }

            return foundFlights;
        }
    }
}
