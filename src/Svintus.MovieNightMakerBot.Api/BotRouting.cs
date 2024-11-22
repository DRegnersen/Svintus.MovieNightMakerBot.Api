using Microsoft.AspNetCore.Mvc;
using Svintus.MovieNightMakerBot.Api.Endpoints;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Api;

internal static class BotRouting
{
    public static void Endpoints(IEndpointRouteBuilder builder)
    {
        var routeBuilder = builder
            .MapGroup("/api")
            .WithTags("MovieNightMakerBot")
            .WithOpenApi();

        routeBuilder
            .MapGet("/status", () => "Telegram bot is running")
            .WithName("Get");
        
        routeBuilder
            .MapPost("/update", (
                    [FromBody] Update update,
                    [FromServices] BotEndpoint endpoint,
                    CancellationToken cancellationToken)
                => endpoint.InvokeAsync(update, cancellationToken))
            .WithName("Post");
    }
}