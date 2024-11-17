using Microsoft.AspNetCore.Mvc;
using Svintus.MovieNightMakerBot.Api.Endpoints;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Api;

internal static class BotRouting
{
    private const string EmptyRoute = "/";

    public static void Endpoints(IEndpointRouteBuilder builder)
    {
        var routeBuilder = builder
            .MapGroup(EmptyRoute)
            .WithTags("MovieNightMakerBot")
            .WithOpenApi();

        routeBuilder
            .MapGet(EmptyRoute, () => "Telegram bot is running")
            .WithName("Get");
        
        routeBuilder
            .MapPost(EmptyRoute, (
                    [FromBody] Update update,
                    [FromServices] BotEndpoint endpoint,
                    CancellationToken cancellationToken)
                => endpoint.InvokeAsync(update, cancellationToken))
            .WithName("Post");
    }
}