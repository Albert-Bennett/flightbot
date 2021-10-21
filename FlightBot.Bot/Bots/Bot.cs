using FlightBot.Services.Abstractions;
using FlightBot.Services.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlightBot.Bot.Bots
{
    public class Bot : ActivityHandler
    {
        private readonly BotState _conversationState;
        private readonly BotState _userState;
        private readonly IStateManagerService _stateManager;

        public Bot(ConversationState conversationState, UserState userState,
            IStateManagerService stateManager)
        {
            _conversationState = conversationState;
            _userState = userState;
            _stateManager = stateManager;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendCurrentState(turnContext, cancellationToken);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendCurrentState(turnContext, cancellationToken);
        }

        private async Task SendCurrentState(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            var adaptiveCard = await _stateManager.GenerateCurentState(userProfile, conversationData,
                turnContext, cancellationToken);

            await turnContext.SendActivityAsync(MessageFactory.Attachment(adaptiveCard));
        }
    }
}
