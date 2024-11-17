using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public abstract class CommandBase : ICommand
{
    public string? Name => GetCommandName();

    public abstract Task ExecuteAsync(Update update);

    private string? GetCommandName()
    {
        var attribute = (CommandNameAttribute?)Attribute.GetCustomAttribute(
            GetType(),
            typeof(CommandNameAttribute)
        );

        return attribute?.Name;
    }
}