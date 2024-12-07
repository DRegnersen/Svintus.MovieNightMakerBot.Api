using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svintus.MovieNightMakerBot.Application.Models.Options;
using Svintus.MovieNightMakerBot.Application.Services;
using Svintus.MovieNightMakerBot.Application.Services.Abstractions;
using Svintus.MovieNightMakerBot.Core.DependencyInjection;

namespace Svintus.MovieNightMakerBot.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotCommands(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddBotCore(configuration);
        services.AddBotCommands();
            
        services
            .Configure<MovieServiceOptions>(configuration.GetSection("Services:Movies"))
            .Configure<RateOptions>(configuration.GetSection("Services:Rates"));

        services.AddScoped<IMovieService, MovieService>();

        return services;
    }
}