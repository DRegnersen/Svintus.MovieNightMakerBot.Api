namespace Svintus.MovieNightMakerBot.Core.Commands;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CommandNameAttribute(string name) : Attribute
{
    public string CommandName { get; } = name;
}