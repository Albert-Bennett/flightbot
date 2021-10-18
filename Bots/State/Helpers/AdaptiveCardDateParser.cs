using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace FlightBot.Bots.State.Helpers
{
    public static class AdaptiveCardDateParser
    {
        public static DateTime GetDatefromUserInput(string userInput)
        {
            var jobject = JObject.Parse(userInput)["dateInput"];
            var dateString = jobject.ToString();

            return DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
