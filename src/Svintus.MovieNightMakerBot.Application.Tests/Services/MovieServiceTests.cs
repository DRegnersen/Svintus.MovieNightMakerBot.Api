using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Svintus.MovieNightMakerBot.Application.Models.Options;
using Svintus.MovieNightMakerBot.Application.Services;
using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Client.Abstractions;
using Svintus.MovieNightMakerBot.Integrations.MoviesMicroservice.Models;
using Svintus.MovieNightMakerBot.XunitKit.AutoFixture.Moq;
using Xunit;

namespace Svintus.MovieNightMakerBot.Application.Tests.Services;

public sealed class MovieServiceTests
{
    [Theory]
    [AutoMoqData]
    internal async Task GetRandomMoviesAsync_ClientReturnedMovies_ReturnsMovies(
        MovieModel[] movies,
        [Frozen] Mock<IMoviesMicroserviceClient> client,
        [Frozen] MovieServiceOptions options,
        MovieService sut)
    {
        // Arrange
        client
            .Setup(m => m.GetRandomMoviesAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movies);

        // Act
        var result = await sut.GetRandomMoviesAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(movies);
        client.Verify(m => m.GetRandomMoviesAsync(options.RandomMoviesNumber, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    internal async Task RateMoviesAsync_RatesWerePassedToClient_AwaitsClient(
        long chatId,
        MovieRateModel[] rates,
        [Frozen] Mock<IMoviesMicroserviceClient> client,
        MovieService sut)
    {
        // Arrange
        client
            .Setup(m => m.RateMoviesAsync(It.IsAny<long>(), It.IsAny<MovieRateModel[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.RateMoviesAsync(chatId, rates, CancellationToken.None);

        // Assert
        client.Verify(m => m.RateMoviesAsync(chatId, rates, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_ClientReturnedSomeError_ReturnsError(
        long chatId,
        Error clientError,
        [Frozen] Mock<IMoviesMicroserviceClient> client,
        [Frozen] MovieServiceOptions options,
        MovieService sut)
    {
        // Arrange
        client
            .Setup(m => m.GetRecommendedMoviesAsync(It.IsAny<long>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientError);

        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId, CancellationToken.None);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Should().Be(clientError);
        client.Verify(m => m.GetRecommendedMoviesAsync(chatId, options.RecommendedMoviesNumber, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_ClientReturnedChatIdError_ReturnsError(
        long chatId,
        Error clientError,
        [Frozen] Mock<IMoviesMicroserviceClient> client,
        [Frozen] MovieServiceOptions options,
        MovieService sut)
    {
        // Arrange
        clientError.Code = ResultCode.ChatIdNotFound;

        client
            .Setup(m => m.GetRecommendedMoviesAsync(It.IsAny<long>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientError);

        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId, CancellationToken.None);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Message.Should().Be("You should rate some movies first");
        client.Verify(m => m.GetRecommendedMoviesAsync(chatId, options.RecommendedMoviesNumber, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    internal async Task GetRecommendedMoviesAsync_EverythingIsOk_ReturnsMovies(
        long chatId,
        MovieModel[] movies,
        [Frozen] Mock<IMoviesMicroserviceClient> client,
        [Frozen] MovieServiceOptions options,
        MovieService sut)
    {
        // Arrange
        client
            .Setup(m => m.GetRecommendedMoviesAsync(It.IsAny<long>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movies);

        // Act
        var result = await sut.GetRecommendedMoviesAsync(chatId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(movies);
        client.Verify(m => m.GetRecommendedMoviesAsync(chatId, options.RecommendedMoviesNumber, It.IsAny<CancellationToken>()), Times.Once);
    }
}