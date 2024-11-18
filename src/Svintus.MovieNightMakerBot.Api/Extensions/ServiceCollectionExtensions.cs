using Svintus.MovieNightMakerBot.Api.Endpoints;
using Svintus.MovieNightMakerBot.Api.Endpoints.Abstractions;
using Svintus.MovieNightMakerBot.Core.DependencyInjection.Extensions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotCore(configuration);
        services.AddScoped<IBotEndpoint<Update>, BotEndpoint>();

        return services;
    }
}