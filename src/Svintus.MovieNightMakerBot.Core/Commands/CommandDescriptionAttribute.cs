namespace Svintus.MovieNightMakerBot.Core.Commands;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandDescriptionAttribute(string description) : Attribute
{
    public string CommandDescription { get; } = description;
}