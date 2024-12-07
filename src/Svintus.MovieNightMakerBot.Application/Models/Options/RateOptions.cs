namespace Svintus.MovieNightMakerBot.Application.Models.Options;

internal sealed class RateOptions
{
    public int MinRate { get; set; }
    public int RatesNumber { get; set; }

    public int MaxRate => MinRate + RatesNumber - 1;
}