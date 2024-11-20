using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Application.Commands;
using Svintus.MovieNightMakerBot.Application.Commands.Contexts;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.DependencyInjection.Extensions;

namespace Svintus.MovieNightMakerBot.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotCommands(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBotCore(configuration);

        services.AddSingleton<ICommand, StartCommand>();
        
        services
            .AddComplexCommand<RatingContext>()
            .WithStep<FirstRateCommand>()
            .WithStep<SecondRateCommand>()
            .WithStep<LastRateCommand>()
            .Build();
        
        return services;
    }
}