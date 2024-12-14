using FluentAssertions;
using Svintus.MovieNightMakerBot.Application.Extensions;
using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;
using Svintus.MovieNightMakerBot.XunitKit.AutoFixture.Moq;
using Xunit;

namespace Svintus.MovieNightMakerBot.Application.Tests.Extensions;

public class ResultExtensionsTests
{
    private const string TestErrorMessage = "Test Error";

    [Theory]
    [AutoMoqData]
    internal void MapError_ResultIsSuccessful_ReturnsSameValue(SuccessfulResultValueStub resultValue)
    {
        // Arrange
        var successfulResult = new Result<SuccessfulResultValueStub, Error>(resultValue);

        // Act
        var result = successfulResult.MapError(m => new Error(m.Code, TestErrorMessage));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(resultValue);
    }

    [Theory]
    [AutoMoqData]
    internal void MapError_ResultIsFailed_ReturnsMappedError(Error resultError)
    {
        // Arrange
        var failedResult = new Result<SuccessfulResultValueStub, Error>(resultError);

        // Act
        var result = failedResult.MapError(m => new Error(m.Code, TestErrorMessage));

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Code.Should().Be(resultError.Code);
        result.Error.Message.Should().Be(TestErrorMessage);
    }

    internal sealed class SuccessfulResultValueStub
    {
        public string? StubField { get; set; }
    }
}