using Svintus.MovieNightMakerBot.Application.Commands.Contexts;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/rate")]
internal sealed class RateCommand(ITelegramBotClient client, IUpdateDistributor distributor) : ComplexCommandBase<RateContext>(distributor)
{
    protected override async Task<CommandStatus> ExecuteCoreAsync(Update update)
    {
        var chatId = update.Message!.Chat.Id;
        
        if (CurrentStep[chatId] == 1)
        {
            Context[chatId].CurrentFilm = FilmSelector.Random();
            await client.SendMessage(chatId, $"How do you rate '{Context[chatId].CurrentFilm}'?");
            
            return CommandStatus.Continue;
        }

        if (CurrentStep[chatId] == 2)
        {
            var rate = Convert.ToInt32(update.Message!.Text);
            if (rate >= Context[chatId].BestRate)
            {
                Context[chatId].BestRate = rate;
                Context[chatId].BestFilm = Context[chatId].CurrentFilm;
            }
            
            Context[chatId].CurrentFilm = FilmSelector.Random();
            await client.SendMessage(chatId, $"How do you rate '{Context[chatId].CurrentFilm}'?");
            
            return CommandStatus.Continue;
        }

        if (CurrentStep[chatId] == 3)
        {
            var rate = Convert.ToInt32(update.Message!.Text);
            if (rate >= Context[chatId].BestRate)
            {
                Context[chatId].BestFilm = Context[chatId].CurrentFilm;
            }
            
            await client.SendMessage(chatId, $"Best rated film is '{Context[chatId].BestFilm}'");
        }

        return CommandStatus.Stop;
    }
}