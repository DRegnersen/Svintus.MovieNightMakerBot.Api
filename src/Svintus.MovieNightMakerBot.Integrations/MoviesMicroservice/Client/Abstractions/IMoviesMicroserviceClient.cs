using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;

public interface IMoviesMicroserviceClient
{
    Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null, CancellationToken cancellationToken = default);

    Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken = default);

    Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null, CancellationToken cancellationToken = default);
}