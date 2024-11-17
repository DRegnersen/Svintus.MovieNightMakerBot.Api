using Svintus.MovieNightMakerBot.Api.Endpoints.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Api.Endpoints;

internal sealed class BotEndpoint(IUpdateDistributor updateDistributor): IBotEndpoint<Update>
{
    public async ValueTask InvokeAsync(Update update, CancellationToken cancellationToken)
    {
        await updateDistributor.HandleUpdateAsync(update);
    }
}