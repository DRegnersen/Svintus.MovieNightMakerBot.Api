namespace Svintus.MovieNightMakerBot.Application;

internal static class FilmSelector
{
    private static readonly List<string> Films =
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
        var index = RandomGenerator.Next(Films.Count);
        return Films[index];
    }
}