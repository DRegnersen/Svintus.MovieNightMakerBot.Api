using Svintus.MovieNightMakerBot.Application.Commands.Contexts;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/rate")]
[CommandDescription("Rate some movies")]
internal sealed class RateCommand(ITelegramBotClient client, IUpdateDistributor distributor) : ComplexCommandBase<RateContext>(distributor)
{
    protected override async Task<CommandStatus> ExecuteCoreAsync(Update update)
    {
        var chatId = update.Message!.Chat.Id;
        
        if (Step[chatId].IsInitial())
        {
            Context[chatId].CurrentMovie = MovieSelector.Random();
            await client.SendMessage(chatId, $"How do you rate '{Context[chatId].CurrentMovie}'?");
            
            return CommandStatus.Continue;
        }

        if (Step[chatId].IsAt(2))
        {
            var rate = Convert.ToInt32(update.Message!.Text);
            if (rate >= Context[chatId].BestRate)
            {
                Context[chatId].BestRate = rate;
                Context[chatId].BestMovie = Context[chatId].CurrentMovie;
            }
            
            Context[chatId].CurrentMovie = MovieSelector.Random();
            await client.SendMessage(chatId, $"How do you rate '{Context[chatId].CurrentMovie}'?");
            
            return CommandStatus.Continue;
        }

        if (Step[chatId].IsAt(3))
        {
            var rate = Convert.ToInt32(update.Message!.Text);
            if (rate >= Context[chatId].BestRate)
            {
                Context[chatId].BestMovie = Context[chatId].CurrentMovie;
            }
            
            await client.SendMessage(chatId, $"Best rated movie is '{Context[chatId].BestMovie}'");
        }

        return CommandStatus.Stop;
    }
}