using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.UpdateDistribution.Abstractions;

public interface IUpdateListener
{
    Task HandleUpdateAsync(Update update);
}