using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotClient(configuration);
        
        services
            .AddSingleton<CommandMediator>()
            .AddSingleton<IUpdateDistributor, UpdateDistributor>(
                sp => new UpdateDistributor(sp.GetRequiredService<CommandMediator>())
            );

        return services;
    }
    
    private static IServiceCollection AddBotClient(this IServiceCollection services, IConfiguration configuration)
    {
        var botOptions = configuration.GetSection("Services:TelegramBotClient").Get<TelegramBotClientOptions>()!;

        services.AddScoped<ITelegramBotClient, TelegramBotClient>(_ => new TelegramBotClient(botOptions));
        
        return services;
    }
}