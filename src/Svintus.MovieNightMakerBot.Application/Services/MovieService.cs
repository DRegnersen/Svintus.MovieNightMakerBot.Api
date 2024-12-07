using Microsoft.Extensions.Options;
using Svintus.MovieNightMakerBot.Application.Extensions;
using Svintus.MovieNightMakerBot.Application.Models.Options;
using Svintus.MovieNightMakerBot.Application.Services.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Application.Services;

internal sealed class MovieService(IMoviesMicroserviceClient client, IOptions<MovieServiceOptions> options) : IMovieService
{
    private readonly MovieServiceOptions _options = options.Value;

    public async Task<MovieModel[]> GetRandomMoviesAsync(CancellationToken cancellationToken)
    {
        return await client.GetRandomMoviesAsync(_options.RandomMoviesNumber, cancellationToken);
    }

    public async Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken)
    {
        await client.RateMoviesAsync(chatId, rates, cancellationToken);
    }

    public async Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, CancellationToken cancellationToken)
    {
        var result = await client.GetRecommendedMoviesAsync(chatId, _options.RecommendedMoviesNumber, cancellationToken);

        return result.MapError(MapChatIdNotFound);
    }

    private static Error MapChatIdNotFound(Error error)
    {
        if (error.Code == ResultCode.ChatIdNotFound)
        {
            return new Error(ResultCode.ChatIdNotFound, "You should rate some movies first");
        }

        return error;
    }
}