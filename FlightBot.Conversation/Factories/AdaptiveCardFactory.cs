using AdaptiveCards;
using FlightBot.Conversation.Factories.Abstractions;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlightBot.Conversation.Factories
{
    public class AdaptiveCardFactory : IAdaptiveCardFactory
    {
        readonly AdaptiveSchemaVersion defaultSchema = new(1, 0);

        public Attachment GetFoundFlightsCard(ICollection<FlightCardData> flightCardData)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new (),
                Actions = new ()
            };

            foreach (var flight in flightCardData)
            {
                card.Body.AddRange(CreateFlightCard(flight));
                card.Actions.Add(new AdaptiveOpenUrlAction
                {
                    Url = new Uri("https://www.google.ie"),
                    Title = $"Book the Flight for {flight.Currency} {flight.MaxPrice} on {flight.DepartureDate}"
                });
            }

            card.Actions.Add(new AdaptiveSubmitAction
            {
                Title = Messages.NO_SUITABLE_FLIGHTS,
                Data = Messages.NO_SUITABLE_FLIGHTS
            });

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        List<AdaptiveElement> CreateFlightCard(FlightCardData flightData)
        {
            string stopDetails = flightData.StopDetails[0].Segments.Count == 1 ?
                " and is non stop to the destination." : 
                $", has {flightData.StopDetails[0].Segments.Count - 1} stops.";

            string description = $"This flight costs {flightData.Currency} {flightData.MaxPrice}{stopDetails}";

            var flightCardElements = new List<AdaptiveElement>
            {
                new AdaptiveTextBlock
                {
                        Size = AdaptiveTextSize.Medium,
                        Weight = AdaptiveTextWeight.Bolder,
                        Text = $"{flightData.AirportIATACode} to {flightData.DestinationIATACode}"
                },
                new AdaptiveColumnSet
                {
                    Columns = new List<AdaptiveColumn>
                    {
                        new AdaptiveColumn
                        {
                            Items = new List<AdaptiveElement>
                            {
                                    new AdaptiveImage
                                    {
                                        Style = AdaptiveImageStyle.Person,
                                        Url = new Uri($"{Environment.CurrentDirectory}/airplane-icon.png"),
                                        Size = AdaptiveImageSize.Medium
                                    }
                            },
                            Width = "auto"
                        },
                        new AdaptiveColumn
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new AdaptiveTextBlock
                                {
                                    Weight = AdaptiveTextWeight.Bolder,
                                    Wrap = true,
                                    Text = $"Your flight from {flightData.Airport} to {flightData.Destination}"
                                },
                                new AdaptiveTextBlock
                                {
                                    Spacing= AdaptiveSpacing.None,
                                    IsSubtle = true,
                                    Wrap = true,
                                    Text = $"Departure Date: {flightData.DepartureDate:dd-MM-yyyy HH:mm}"
                                }
                            },
                            Width = "stretch"
                        }
                    }
                }
            };

            foreach (StopDetails stop in flightData.StopDetails)
            {
                flightCardElements.Add(new AdaptiveTextBlock
                {
                    Text = $"Duration: {stop.Duration}",
                    Wrap = true
                });

                foreach (var stopSegment in stop.Segments)
                {
                    flightCardElements.Add(CreateFlightStopDetails(stopSegment));
                }
            }

            flightCardElements.Add(new AdaptiveTextBlock
            {
                Text = description,
                Wrap = true
            });

            return flightCardElements;
        }

        AdaptiveColumnSet CreateFlightStopDetails(StopSegment stopDetails)
        {
            string arrivalText = $"Arrival time: {stopDetails.ArivialDate}. Arriving at: {stopDetails.ArivialIATACode}";
            arrivalText += stopDetails.ArivialTerminal == null ? "." : $", terminal: {stopDetails.ArivialTerminal}.";

            string departureText = $"Departure time: {stopDetails.DepartureDate}. Departing from: {stopDetails.DepartureIATACode}";
            departureText += stopDetails.DepartureTerminal == null ? "." : $", terminal: {stopDetails.DepartureTerminal}.";

            return new AdaptiveColumnSet
            {
                Columns = new List<AdaptiveColumn>
            {
                new AdaptiveColumn
                {
                    Items = new List<AdaptiveElement>
                    {
                            new AdaptiveImage
                            {
                                Style = AdaptiveImageStyle.Person,
                                Url = new Uri($"{Environment.CurrentDirectory}/airplane-icon.png"),
                                Size = AdaptiveImageSize.Small
                            }
                    },
                    Width = "auto"
                },
                new AdaptiveColumn
                {
                    Items = new List<AdaptiveElement>
                    {
                        new AdaptiveTextBlock
                        {
                            Weight = AdaptiveTextWeight.Bolder,
                            Wrap = true,
                            Text = $"Duration: {stopDetails.Duration}."
                        },
                        new AdaptiveTextBlock
                        {
                            Spacing= AdaptiveSpacing.None,
                            IsSubtle = true,
                            Wrap = true,
                            Text = departureText
                        },
                        new AdaptiveTextBlock
                        {
                            Spacing= AdaptiveSpacing.None,
                            IsSubtle = true,
                            Wrap = true,
                            Text = arrivalText
                        }
                    },
                    Width = "stretch"
                }
                }
            };
        }

        public Attachment GetOptionalCalanderCard(string message)
        {
            AdaptiveCard card = CreateCalanderCard(message);
            card.Actions.Add(new AdaptiveSubmitAction()
            {
                Title = Messages.DECLINE,
                Data = Messages.DECLINE
            });

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public Attachment GetCalanderCard(string message)
        {
            return CreateAdaptiveCardAttachment(CreateCalanderCard(message).ToJson());
        }

        private AdaptiveCard CreateCalanderCard(string message)
        {
            return new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true
                    },

                    new AdaptiveDateInput()
                    {
                        Value = DateTime.Now.ToShortDateString(),
                        Placeholder =  "Enter a Date",
                        Id = "dateInput"
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Type = "Action.Submit",
                        Title = "Submit"
                    }
                }
            };
        }

        public Attachment GetTextCard(string message)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public Attachment GetActionCard(string message, string action)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true
                    },
                    new AdaptiveActionSet
                    {
                        Actions = new List<AdaptiveAction>
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = action,
                                Data = action
                            }
                        }
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public Attachment GetCaroselCard(string message, ICollection<string> data)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true
                    }
                }
            };

            List<string> actions = new(data);

            actions.Add(Messages.NONE_OF_THESE);

            foreach (var action in actions)
            {
                card.Body.Add(new AdaptiveActionSet()
                {
                    Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction(){
                        Title = action,
                        Data = action
                    }
                }
                });
            }

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        private Attachment CreateAdaptiveCardAttachment(string jsonData)
        {
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(jsonData),
            };

            return adaptiveCardAttachment;
        }
    }
}
