using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FlightBot.Bots.State
{
    public class AdaptiveCardFactory
    {
        static AdaptiveSchemaVersion defaultSchema = new AdaptiveSchemaVersion(1, 0);

        public static Attachment GetCalanderCard(string message)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default
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

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public static Attachment GetTextCard(string message)
        {
            AdaptiveCard card = new(defaultSchema)
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock()
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default
                    }
                }
            };

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public static Attachment GetActionCard(string message, string action)
        {
            AdaptiveCard card = new AdaptiveCard(defaultSchema)
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default
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

        public static Attachment GetCaroselCard(string message, IList<string> actions)
        {
            AdaptiveCard card = new AdaptiveCard(defaultSchema)
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = message,
                        Size = AdaptiveTextSize.Default
                    }
                }
            };

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

        private static Attachment CreateAdaptiveCardAttachment(string jsonData)
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
