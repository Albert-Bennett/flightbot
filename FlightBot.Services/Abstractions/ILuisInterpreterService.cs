using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface ILuisInterpreterService : IRecognizer
    {
        Task<string> InterpretDestination(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
