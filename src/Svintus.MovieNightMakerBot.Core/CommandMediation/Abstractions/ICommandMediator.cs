using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.CommandMediation.Abstractions;

public interface ICommandMediator
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken = default);
}