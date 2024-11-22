using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public interface ICommand
{
    string? CommandName { get; }
    
    string? CommandDescription { get; }
    
    Task ExecuteAsync(Update update);
}