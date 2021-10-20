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

        public static Attachment GetOptionalCalanderCard(string message)
        {
            AdaptiveCard card = CreateCalanderCard(message);
            card.Actions.Add(new AdaptiveSubmitAction()
            {
                Title = Messages.DECLINE,
                Data = Messages.DECLINE
            });

            return CreateAdaptiveCardAttachment(card.ToJson());
        }

        public static Attachment GetCalanderCard(string message)
        {
            return CreateAdaptiveCardAttachment(CreateCalanderCard(message).ToJson());
        }

        private static AdaptiveCard CreateCalanderCard(string message)
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

        public static Attachment GetTextCard(string message)
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

        public static Attachment GetActionCard(string message, string action)
        {
            AdaptiveCard card = new AdaptiveCard(defaultSchema)
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

        public static Attachment GetCaroselCard(string message, IList<string> actions)
        {
            AdaptiveCard card = new AdaptiveCard(defaultSchema)
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
