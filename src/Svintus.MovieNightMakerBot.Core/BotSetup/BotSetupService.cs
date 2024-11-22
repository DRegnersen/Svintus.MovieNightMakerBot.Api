using Microsoft.Extensions.Options;
using Svintus.MovieNightMakerBot.Core.BotSetup.Abstractions;
using Svintus.MovieNightMakerBot.Core.BotSetup.Models;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.BotSetup;

internal sealed class BotSetupService(ITelegramBotClient client, IEnumerable<ICommand> commands, IOptions<BotSetupOptions> options)
    : IBotSetupService
{
    public async Task SetupAsync()
    {
        await SetupCommandsAsync();
        
        if (options.Value.WebhookUrl is not null)
        {
            await client.SetWebhook(options.Value.WebhookUrl);
        }
    }

    private async Task SetupCommandsAsync()
    {
        var botCommands = commands
            .Where(c => c.CommandName is not null && c.CommandDescription is not null)
            .Select(c => new BotCommand
            {
                Command = c.CommandName!,
                Description = c.CommandDescription!
            });

        await client.SetMyCommands(botCommands);
    }
}