using FlightBot.Services.Abstractions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace FlightBot.Services
{
    public class LuisInterpreterService : ILuisInterpreterService
    {
        LuisRecognizer luisRecognizer;

        public LuisInterpreterService(IConfiguration configuration)
        {
            var luisApp = new LuisApplication(configuration["LuisAppId"],
                configuration["LuisAppSubscriptionKey"],
                configuration["LuisEndpoint"]);

            var recognizerOptions = new LuisRecognizerOptionsV3(luisApp);

            luisRecognizer = new LuisRecognizer(recognizerOptions);
        }

        public async Task<string> InterpretDestination(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var interpretation = await RecognizeAsync(turnContext, cancellationToken);

            return interpretation.Entities.ContainsKey("geographyV2") ?
                interpretation.Entities["geographyV2"].First["location"].ToString() :
                string.Empty;
        }

        public async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken) =>
            await luisRecognizer.RecognizeAsync(turnContext, cancellationToken);

        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken) where T : IRecognizerConvert, new() =>
            await luisRecognizer.RecognizeAsync<T>(turnContext, cancellationToken);
    }
}
