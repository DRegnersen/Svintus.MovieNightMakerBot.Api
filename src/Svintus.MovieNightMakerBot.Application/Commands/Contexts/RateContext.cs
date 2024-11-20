namespace Svintus.MovieNightMakerBot.Application.Commands.Contexts;

internal sealed class RateContext
{
    public int BestRate { get; set; }
    public string? BestFilm { get; set; }
    public string? CurrentFilm { get; set; }
}