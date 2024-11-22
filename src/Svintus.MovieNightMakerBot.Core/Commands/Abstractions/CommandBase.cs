using Telegram.Bot.Types;

namespace Svintus.MovieNightMakerBot.Core.Commands.Abstractions;

public abstract class CommandBase : ICommand
{
    public string? CommandName => GetCommandName();

    public string? CommandDescription => GetCommandDescription();

    public abstract Task ExecuteAsync(Update update);

    private string? GetCommandName()
    {
        var attribute = (CommandNameAttribute?)Attribute.GetCustomAttribute(
            GetType(),
            typeof(CommandNameAttribute)
        );

        return attribute?.CommandName;
    }
    
    private string? GetCommandDescription()
    {
        var attribute = (CommandDescriptionAttribute?)Attribute.GetCustomAttribute(
            GetType(),
            typeof(CommandDescriptionAttribute)
        );

        return attribute?.CommandDescription;
    }
}