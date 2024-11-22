namespace Svintus.MovieNightMakerBot.Application.Commands.Contexts;

internal sealed class RateContext
{
    public int BestRate { get; set; }
    public string? BestMovie { get; set; }
    public string? CurrentMovie { get; set; }
}