using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace FlightBot.Conversation.Extensions
{
    public static class CalanderDateTimeExtension
    {
        public static DateTime GetDatefromCalander(this string userInput)
        {
            var jobject = JObject.Parse(userInput)["dateInput"];
            var dateString = jobject.ToString();

            return DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
