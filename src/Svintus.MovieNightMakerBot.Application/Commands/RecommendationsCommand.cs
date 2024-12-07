using System.Text;
using Svintus.MovieNightMakerBot.Application.Services.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/recommendations")]
[CommandDescription("Get recommended movies")]
internal sealed class RecommendationsCommand(ITelegramBotClient client, IMovieService movieService) : CommandBase
{
    public override async Task ExecuteAsync(Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        
        var moviesResult = await movieService.GetRecommendedMoviesAsync(chatId, ct);
        if (moviesResult.IsFail)
        {
            await client.SendMessage(chatId, moviesResult.Error.Message, cancellationToken: ct);
            return;
        }

        var message = new StringBuilder();
        foreach (var movie in moviesResult.Value)
        {
            message.AppendLine($"- *{movie.Title}*");
        }
        
        await client.SendMessage(chatId, message.ToString(), cancellationToken: ct);
    }
}