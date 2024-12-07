using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Core.BotSetup;
using Svintus.MovieNightMakerBot.Core.BotSetup.Abstractions;
using Svintus.MovieNightMakerBot.Core.BotSetup.Models.Options;
using Svintus.MovieNightMakerBot.Core.CommandMediation;
using Svintus.MovieNightMakerBot.Core.CommandMediation.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;

namespace Svintus.MovieNightMakerBot.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotCommands(this IServiceCollection services)
    {
        var commands = Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var command in commands)
        {
            services.AddSingleton(typeof(ICommand), command);
        }

        return services;
    }
    
    public static IServiceCollection AddBotCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotClient(configuration);

        services.AddSingleton<IBotSetupService, BotSetupService>();
        
        services.AddSingleton<IUpdateDistributor, UpdateDistributor>();
        services.AddSingleton<ICommandMediator, CommandMediator>();

        return services;
    }

    private static IServiceCollection AddBotClient(this IServiceCollection services, IConfiguration configuration)
    {
        var botOptions = configuration.GetSection("Services:TelegramBotClient").Get<TelegramBotClientOptions>()!;

        services
            .AddSingleton<ITelegramBotClient>(new TelegramBotClient(botOptions))
            .Configure<BotSetupOptions>(configuration.GetSection("Services:TelegramBotClient"));
        
        return services;
    }
}