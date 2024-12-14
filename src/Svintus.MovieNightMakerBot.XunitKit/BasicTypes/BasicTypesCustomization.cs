using AutoFixture;

namespace Svintus.MovieNightMakerBot.XunitKit.BasicTypes;

public class BasicTypesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }
}