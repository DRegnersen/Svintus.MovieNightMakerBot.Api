using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public interface ICommand
{
    string? Name { get; }
    
    Task ExecuteAsync(Update update);
}