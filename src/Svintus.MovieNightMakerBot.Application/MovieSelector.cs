namespace Svintus.MovieNightMakerBot.Application;

internal static class MovieSelector
{
    private static readonly List<string> Movies =
    [
        "The Shawshank Redemption",
        "The Godfather",
        "The Dark Knight",
        "Pulp Fiction",
        "The Lord of the Rings: The Return of the King",
        "Forrest Gump",
        "Inception",
        "Fight Club",
        "The Matrix",
        "Goodfellas"
    ];

    private static readonly Random RandomGenerator = new();

    public static string Random()
    {
        var index = RandomGenerator.Next(Movies.Count);
        return Movies[index];
    }
}