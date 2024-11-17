namespace Svintus.MovieNightMakerBot.Api.Endpoints.Abstractions;

internal interface IBotEndpoint<in TUpdate>
{
    ValueTask InvokeAsync(TUpdate update, CancellationToken cancellationToken);
}