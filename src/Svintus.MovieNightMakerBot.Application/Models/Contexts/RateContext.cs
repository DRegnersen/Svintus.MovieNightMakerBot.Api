namespace Svintus.MovieNightMakerBot.Application.Models.Contexts;

internal sealed class RateContext
{
    public MovieRate[] Rates { get; set; } = [];
}

internal sealed class MovieRate
{
    public long MovieId { get; set; }
    public int Rate { get; set; }
}