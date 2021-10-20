using FlightBot.Services.Abstractions;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class TextExtractorService : ITextExtractorService
    {
        public async Task<string> ExtractDestination(string userInput)
        {
            return "Canada";
        }
    }
}
