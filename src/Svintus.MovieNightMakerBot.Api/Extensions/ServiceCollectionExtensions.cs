using Svintus.MovieNightMakerBot.Api.Endpoints;
using Svintus.MovieNightMakerBot.Application.Extensions;

namespace Svintus.MovieNightMakerBot.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotCommands(configuration);
        services.AddSingleton<BotEndpoint>();

        return services;
    }
}