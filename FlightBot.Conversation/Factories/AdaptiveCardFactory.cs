using AdaptiveCards;
using FlightBot.Conversation.Factories.Abstractions;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlightBot.Conversation.Factories
{
    public class AdaptiveCardFactory : IAdaptiveCardFactory
    {
        readonly AdaptiveSchemaVersion defaultSchema = new(1, 0);

        public Attachment GetFoundFlightsCard(FlightCardData flightCardData)
        {
            string description = "";

            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>
                    {
                        new AdaptiveTextBlock
                        {
                             Size = AdaptiveTextSize.Medium,
                             Weight = AdaptiveTextWeight.Bolder,
                             Text = $"{flightCardData.AirportIATACode} to {flightCardData.DestinationIATACode}"
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
                                           Text = $"Your flight from {flightCardData.Airport} to {flightCardData.Destination}"
                                       },
                                       new AdaptiveTextBlock
                                       {
                                           Spacing= AdaptiveSpacing.None,
                                           IsSubtle = true,
                                           Wrap = true,
                                           Text = $"Departure Date: {flightCardData.DepartureDate:dd-MM-yyyy HH:mm}"
                                       }
                                   },
                                   Width = "stretch"
                               }
                           }
                        },
                        new AdaptiveTextBlock
                        {
                            Text = description,
                             Wrap = true
                        }
                    },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveOpenUrlAction
                    {
                        Url = new Uri("https://www.google.ie"),
                        Title = "Book this flight"
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        //public Attachment GetFoundFlightsCard(string message, ICollection<string> flights)
        //{
        //    var cardActions = new List<AdaptiveAction>();

        //    foreach (var f in flights)
        //    {
        //        cardActions.Add(new AdaptiveOpenUrlAction
        //        {
        //            Title = f,
        //            Url = new Uri(f)
        //        });
        //    }

        //    cardActions.Add(new AdaptiveSubmitAction
        //    {
        //        Title = Messages.NO_SUITABLE_FLIGHTS,
        //        Data = Messages.NO_SUITABLE_FLIGHTS
        //    });

        //    AdaptiveCard card = new(defaultSchema)
        //    {
        //        Body = new List<AdaptiveElement>
        //        {
        //            new AdaptiveTextBlock
        //            {
        //                Text = message,
        //                Size = AdaptiveTextSize.Default,
        //                Wrap = true
        //            },
        //            new AdaptiveActionSet
        //            {
        //                Actions = cardActions
        //            }
        //        }
        //    };

        //    return CreateAdaptiveCardAttachment(card.ToJson());
        //}

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
