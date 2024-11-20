using Svintus.MovieNightMakerBot.Api.Endpoints.Abstractions;
using Svintus.MovieNightMakerBot.Core.CommandMediation.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Api.Endpoints;

internal sealed class BotEndpoint(ICommandMediator commandMediator): IBotEndpoint<Update>
{
    public async ValueTask InvokeAsync(Update update, CancellationToken cancellationToken)
    {
        await commandMediator.HandleUpdateAsync(update);
    }
}