using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Svintus.Microservices.Movies;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;

namespace Svintus.MovieNightMakerBot.Integrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();

        services.AddGrpcClient<MovieService.MovieServiceClient>(o =>
        {
            o.Address = configuration.GetValue<Uri>("GrpcServices:MoviesMicroservice:Url");
        });

        services
            .AddScoped<MoviesMicroserviceClient>()
            .AddScoped<MoviesMicroserviceClientMock>()
            .AddScoped<IMoviesMicroserviceClient>(sp => new MoviesMicroserviceClientProxy(
                sp.GetRequiredService<MoviesMicroserviceClient>(),
                sp.GetRequiredService<MoviesMicroserviceClientMock>(),
                sp.GetRequiredService<IFeatureManager>()
            ));

        return services;
    }
}