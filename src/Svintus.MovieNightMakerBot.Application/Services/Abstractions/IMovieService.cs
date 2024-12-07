using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Application.Services.Abstractions;

internal interface IMovieService
{
    Task<MovieModel[]> GetRandomMoviesAsync(CancellationToken cancellationToken = default);

    Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken = default);

    Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, CancellationToken cancellationToken = default);
}