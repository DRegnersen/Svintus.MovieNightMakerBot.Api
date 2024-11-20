using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.CommandMediation;
using Svintus.MovieNightMakerBot.Core.CommandMediation.Abstractions;
using Svintus.MovieNightMakerBot.Core.DependencyInjection.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    /// <typeparam name="TContext">The type of the complex command context</typeparam>
    public static IComplexCommandBuilder<TContext> AddComplexCommand<TContext>(this IServiceCollection services) where TContext : new()
    {
        return new ComplexCommandBuilder<TContext>(services);
    }

    public static IServiceCollection AddBotCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotClient(configuration);

        services
            .AddSingleton<IUpdateDistributor, UpdateDistributor>()
            .AddSingleton<ICommandMediator, CommandMediator>();

        return services;
    }

    private static IServiceCollection AddBotClient(this IServiceCollection services, IConfiguration configuration)
    {
        var botOptions = configuration.GetSection("Services:TelegramBotClient").Get<TelegramBotClientOptions>()!;

        services.AddSingleton<ITelegramBotClient, TelegramBotClient>(_ => new TelegramBotClient(botOptions));

        return services;
    }
}