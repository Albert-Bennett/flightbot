using FlightBot.Services.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace FlightBot.Services.Abstractions
{
    public interface IStateManagerService
    {
        Task<Attachment> GenerateCurentState(UserProfile userProfile,
            ConversationData conversationData, ITurnContext turnContext);
    }
}
