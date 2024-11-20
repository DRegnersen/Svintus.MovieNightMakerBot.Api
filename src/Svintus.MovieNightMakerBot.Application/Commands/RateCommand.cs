using Svintus.MovieNightMakerBot.Application.Commands.Contexts;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/rate")]
internal sealed class FirstRateCommand(ITelegramBotClient client, IUpdateDistributor distributor, ComplexCommandBase<RatingContext> next) 
    : ComplexCommandBase<RatingContext>(distributor, next)
{
    protected override async Task ExecuteCoreAsync(Update update)
    {
        var chatId = update.Message!.Chat.Id;
        
        Context.CurrentFilm = FilmSelector.Random();
        await client.SendMessage(chatId, $"How do you rate '{Context.CurrentFilm}'?");
    }
}

internal sealed class SecondRateCommand(ITelegramBotClient client, IUpdateDistributor distributor, ComplexCommandBase<RatingContext> next) 
    : ComplexCommandBase<RatingContext>(distributor, next)
{
    protected override async Task ExecuteCoreAsync(Update update)
    {
        var rate = Convert.ToInt32(update.Message!.Text);

        if (rate > Context.BestRate)
        {
            Context.BestRate = rate;
            Context.BestFilm = Context.CurrentFilm;
        }
        
        var chatId = update.Message!.Chat.Id;
        
        Context.CurrentFilm = FilmSelector.Random();
        await client.SendMessage(chatId, $"How do you rate '{Context.CurrentFilm}'?");
    }
}

internal sealed class LastRateCommand(ITelegramBotClient client, IUpdateDistributor distributor) 
    : ComplexCommandBase<RatingContext>(distributor)
{
    protected override async Task ExecuteCoreAsync(Update update)
    {
        var rate = Convert.ToInt32(update.Message!.Text);
        if (rate > Context.BestRate)
        {
            Context.BestFilm = Context.CurrentFilm;
        }
        
        var chatId = update.Message!.Chat.Id;
        await client.SendMessage(chatId, $"Best rated film is '{Context.BestFilm}'");
    }
}