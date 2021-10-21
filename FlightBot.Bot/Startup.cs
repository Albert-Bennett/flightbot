using FlightBot.Services;
using FlightBot.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlightBot.Bot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient().AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddTransient<IBot, Bots.Bot>();
            services.AddTransient<IStateManagerService, StateManagerService>();
            services.AddTransient<IAirportFindingService, AirportFindingService>();
            services.AddTransient<ILuisInterpreterService, LuisInterpreterService>();
            services.AddTransient<IFlightFindingService, FlightFindingService>();

            CreateConversationalState(services);
        }

        private static void CreateConversationalState(IServiceCollection services)
        {
            var storage = new MemoryStorage();

            var userState = new UserState(storage);
            services.AddSingleton(userState);

            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
