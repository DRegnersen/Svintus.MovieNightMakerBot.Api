using Grpc.Core;
using Svintus.Microservices.Movies;
using Svintus.MovieNightMakerBot.Integrations.Common.Extensions;
using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;

namespace Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client;

internal sealed class MoviesMicroserviceClient(MovieService.MovieServiceClient client) : IMoviesMicroserviceClient
{
    public async Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (moviesNumber is < 0)
        {
            throw new ArgumentException("Movies number cannot be negative", nameof(moviesNumber));
        }

        var request = new GetRandomMoviesRequest();
        if (moviesNumber is not null)
        {
            request.MoviesNumber = (uint)moviesNumber.Value;
        }
        
        var response = await client.GetRandomMoviesAsync(request, cancellationToken: cancellationToken);
        
        return response.Movies.Select(m => new MovieModel(m.Id, m.Title)).ToArray();
    }
    
    public async Task RateMoviesAsync(long chatId, MovieRateModel[] rates, CancellationToken cancellationToken = default)
    {
        var request = new RateMoviesRequest { ChatId = chatId };
        
        foreach (var rateModel in rates)
        {
            if (rateModel.Rate < 0)
            {
                throw new ArgumentException("Rate cannot be negative", nameof(rateModel.Rate));
            }
            
            request.Rates.Add(new MovieRate
            {
                MovieId = rateModel.MovieId, 
                Rate = (uint)rateModel.Rate
            });
        }
        
        await client.RateMoviesAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null, CancellationToken cancellationToken = default)
    {
        if (moviesNumber is < 0)
        {
            throw new ArgumentException("Movies number cannot be negative", nameof(moviesNumber));
        }

        var request = new GetRecommendedMoviesRequest { ChatId = chatId };
        if (moviesNumber is not null)
        {
            request.MoviesNumber = (uint)moviesNumber.Value;
        }

        GetRecommendedMoviesResponse? response;

        try
        {
            response = await client.GetRecommendedMoviesAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException exception)
        {
            if (exception.IsBadRequest(StatusCode.NotFound, "chatId"))
            {
                return new Error(ResultCode.ChatIdNotFound);
            }

            throw;
        }

        return response.Movies.Select(m => new MovieModel(m.Id, m.Title)).ToArray();
    }
}