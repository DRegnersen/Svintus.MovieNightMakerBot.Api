using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands;

public class CommandMediator(IEnumerable<ICommand> commands) : IUpdateListener
{
    public async Task HandleUpdateAsync(Update update)
    {
        var messageText = update.Message?.Text;
        if (messageText is null)
            return;

        var command = commands.FirstOrDefault(c => c.Name == messageText);
        if (command is not null)
        {
            await command.ExecuteAsync(update);
        }
    }
}