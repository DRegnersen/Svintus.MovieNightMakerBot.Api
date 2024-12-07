using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client;

internal sealed class MoviesMicroserviceClientMock : IMoviesMicroserviceClient
{
    private const int DefaultMoviesNumber = 5;
    
    public Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (moviesNumber is < 0)
        {
            throw new ArgumentException("Movies number cannot be negative", nameof(moviesNumber));
        }
        
        return Task.FromResult(MoviesGenerator.GetRandomMovies(moviesNumber ?? DefaultMoviesNumber));
    }

    public Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (moviesNumber is < 0)
        {
            throw new ArgumentException("Movies number cannot be negative", nameof(moviesNumber));
        }
        
        return Task.FromResult<Result<MovieModel[], Error>>(MoviesGenerator.GetRandomMovies(moviesNumber ?? DefaultMoviesNumber));
    }
}

file static class MoviesGenerator
{
    #region Movies

    private static readonly List<string> Movies =
    [
        "The Shawshank Redemption",
        "The Godfather",
        "The Dark Knight",
        "Pulp Fiction",
        "The Lord of the Rings: The Return of the King",
        "Forrest Gump",
        "Inception",
        "Fight Club",
        "The Matrix",
        "Goodfellas"
    ];

    #endregion
    
    public static MovieModel[] GetRandomMovies(int moviesNumber)
    {
        var random = new Random();
        var randomMovies = new List<MovieModel>();

        for (var i = 0; i < moviesNumber; i++)
        {
            var randomIndex = random.Next(Movies.Count);
            randomMovies.Add(new MovieModel(randomIndex, Movies[randomIndex]));
        }

        return randomMovies.ToArray();
    }
}