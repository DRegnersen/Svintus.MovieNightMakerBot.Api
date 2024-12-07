using Svintus.MovieNightMakerBot.Core.CommandMediation.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.CommandMediation;

internal sealed class CommandMediator(IEnumerable<ICommand> commands, IUpdateDistributor distributor) : ICommandMediator
{
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.Message is null)
            return;
        
        var chatId = update.Message.Chat.Id;
        if (distributor.TryGetListener(chatId, out var listener))
        {
            await listener.HandleUpdateAsync(update, cancellationToken);
            return;
        }
        
        if (update.Message.Text is null)
            return;
        
        var command = commands.FirstOrDefault(c => c.CommandName == update.Message.Text);
        if (command is not null)
        {
            await command.ExecuteAsync(update, cancellationToken);
        }
    }
}