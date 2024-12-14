using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Svintus.MovieNightMakerBot.XunitKit.BasicTypes;
using Svintus.MovieNightMakerBot.XunitKit.Options;

namespace Svintus.MovieNightMakerBot.XunitKit.AutoFixture.Moq;

public class AutoMoqDataAttribute() : AutoDataAttribute(() => new Fixture().Customize(
    new CompositeCustomization(
        new AutoMoqCustomization { ConfigureMembers = false },
        new OptionsCustomization(),
        new BasicTypesCustomization()
    )
));