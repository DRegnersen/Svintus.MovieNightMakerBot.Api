using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/start")]
[CommandDescription("About this bot")]
internal sealed class StartCommand(ITelegramBotClient client) : CommandBase
{
    public override async Task ExecuteAsync(Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        await client.SendMessage(chatId, "Welcome to MovieNightMakerBot", cancellationToken: ct);
    }
}