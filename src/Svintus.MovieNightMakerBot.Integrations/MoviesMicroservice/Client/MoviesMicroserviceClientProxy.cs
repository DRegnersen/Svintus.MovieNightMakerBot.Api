using Microsoft.FeatureManagement;
using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client;

internal class MoviesMicroserviceClientProxy(IMoviesMicroserviceClient client, IMoviesMicroserviceClient clientMock, IFeatureManager featureManager)
    : IMoviesMicroserviceClient
{
    private const string FeatureFlag = "MoviesMicroserviceIsAvailable";
    
    public async Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            return await client.GetRandomMoviesAsync(moviesNumber, cancellationToken);
        }
        
        return await clientMock.GetRandomMoviesAsync(moviesNumber, cancellationToken);
    }

    public async Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken = default)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            await client.RateMoviesAsync(chatId, rates, cancellationToken);
            return;
        }
        
        await clientMock.RateMoviesAsync(chatId, rates, cancellationToken);
    }

    public async Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (await featureManager.IsEnabledAsync(FeatureFlag))
        {
            return await client.GetRecommendedMoviesAsync(chatId, moviesNumber, cancellationToken);
        }
        
        return await clientMock.GetRecommendedMoviesAsync(chatId, moviesNumber, cancellationToken);
    }
}