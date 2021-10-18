using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface ITextExtractorService
    {
        Task<string> ExtractDestination(string userInput);
    }
}
