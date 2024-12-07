using Microsoft.Extensions.Options;
using Svintus.MovieNightMakerBot.Application.Models.Contexts;
using Svintus.MovieNightMakerBot.Application.Models.Options;
using Svintus.MovieNightMakerBot.Application.Services.Abstractions;
using Svintus.MovieNightMakerBot.Core.Commands;
using Svintus.MovieNightMakerBot.Core.Commands.Abstractions;
using Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Application.Commands;

[CommandName("/rate")]
[CommandDescription("Rate movies to improve recommendations")]
internal sealed class RateCommand(ITelegramBotClient client, IUpdateDistributor distributor, IMovieService movieService, IOptions<RateOptions> options)
    : ComplexCommandBase<RateContext>(distributor)
{
    private readonly RateOptions _options = options.Value;
    
    protected override async Task<CommandStatus> ExecuteCoreAsync(Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;

        if (Step[chatId].IsInitial())
        {
            var movies = await movieService.GetRandomMoviesAsync(ct);
            Context[chatId].Rates = movies.Select(m => new MovieRate { MovieId = m.Id, MovieTitle = m.Title}).ToArray();   
        }
        else
        {
            if (!TryGetRateAsync(update, out var rate))
            {
                await client.SendMessage(chatId, $"You should give a rate from {_options.MinRate} to {_options.MaxRate}", cancellationToken: ct);
                return CommandStatus.Repeat;
            }

            Context[chatId].Rates[Step[chatId].Before()].Rate = rate;
        }
        
        if (Step[chatId] < Context[chatId].Rates.Length)
        {
            await client.SendMessage(chatId, $"How do you rate {Context[chatId].Rates[Step[chatId]].MovieTitle}?", cancellationToken: ct);
            
            return CommandStatus.Continue;
        }

        var finalRates = Context[chatId].Rates.Select(r => new MovieRateModel(r.MovieId, r.Rate)).ToArray();
        await movieService.RateMoviesAsync(chatId, finalRates, ct);
        
        await client.SendMessage(chatId, "Thank you, your rates have been taken on board", cancellationToken: ct);
        
        return CommandStatus.Stop;
    }

    private bool TryGetRateAsync(Update update, out int rate)
    {
        try
        {
            rate = Convert.ToInt32(update.Message!.Text);
        }
        catch (Exception exception)
        {
            if (exception is FormatException or OverflowException)
            {
                rate = 0;
                return false;
            }

            throw;
        }

        return _options.MinRate <= rate && rate <= _options.MaxRate;
    }
}