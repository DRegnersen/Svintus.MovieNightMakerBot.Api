using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Svintus.MovieNightMakerBot.Integrations.Common.Extensions;
using Xunit;

namespace Svintus.MovieNightMakerBot.Integrations.Tests.Common.Extensions;

public sealed class RpcExceptionExtensionsTests
{
    [Theory]
    [MemberData(nameof(TestData.NotBadRequestExceptions), MemberType = typeof(TestData))]
    internal void IsBadRequest_IsNotBadRequest_ReturnsFalse(RpcException rpcException)
    {
        // Act & Assert
        rpcException.IsBadRequest().Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(TestData.ImproperStatusCodeExceptions), MemberType = typeof(TestData))]
    internal void IsBadRequest_UnexpectedStatusCode_ReturnsFalse(RpcException rpcException, StatusCode expectedStatusCode)
    {
        // Act & Assert
        rpcException.IsBadRequest(expectedStatusCode).Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(TestData.NotViolatedFieldExceptions), MemberType = typeof(TestData))]
    internal void IsBadRequest_ExpectedFieldIsNotViolated_ReturnsFalse(
        RpcException rpcException,
        StatusCode expectedStatusCode,
        string expectedViolatedField)
    {
        // Act & Assert
        rpcException.IsBadRequest(expectedStatusCode, expectedViolatedField).Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(TestData.ExpectedBadRequestExceptions), MemberType = typeof(TestData))]
    internal void IsBadRequest_IsExpectedBadRequest_ReturnsTrue(
        RpcException rpcException,
        StatusCode expectedStatusCode,
        string expectedViolatedField)
    {
        // Act & Assert
        rpcException.IsBadRequest(expectedStatusCode, expectedViolatedField).Should().BeTrue();
    }
}

#region TestData

file static class TestData
{
    /// <returns>rpcException</returns>
    public static TheoryData<RpcException> NotBadRequestExceptions() => new()
    {
        new Google.Rpc.Status
        {
            Code = (int)Code.Internal,
            Message = "Internal Server Error",
            Details =
            {
                Any.Pack(new ErrorInfo())
            }
        }.ToRpcException()
    };

    /// <returns>rpcException, expectedStatusCode</returns>
    public static TheoryData<RpcException, StatusCode> ImproperStatusCodeExceptions() => new()
    {
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.InvalidArgument,
                Message = "Invalid argument",
                Details =
                {
                    Any.Pack(new BadRequest())
                }
            }.ToRpcException(),

            StatusCode.NotFound
        },
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.NotFound,
                Message = "Not Found",
                Details =
                {
                    Any.Pack(new BadRequest())
                }
            }.ToRpcException(),

            StatusCode.InvalidArgument
        }
    };

    /// <returns>rpcException, expectedStatusCode, expectedViolatedField</returns>
    public static TheoryData<RpcException, StatusCode, string> NotViolatedFieldExceptions() => new()
    {
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.NotFound,
                Message = "Not Found",
                Details =
                {
                    Any.Pack(new BadRequest())
                }
            }.ToRpcException(),

            StatusCode.NotFound,

            "chatId"
        },
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.InvalidArgument,
                Message = "Invalid Argument",
                Details =
                {
                    Any.Pack(new BadRequest
                    {
                        FieldViolations =
                        {
                            new BadRequest.Types.FieldViolation
                            {
                                Field = "userId"
                            }
                        }
                    })
                }
            }.ToRpcException(),

            StatusCode.InvalidArgument,

            "chatId"
        }
    };
    
    /// <returns>rpcException, expectedStatusCode, expectedViolatedField</returns>
    public static TheoryData<RpcException, StatusCode, string> ExpectedBadRequestExceptions() => new()
    {
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.NotFound,
                Message = "Not Found",
                Details =
                {
                    Any.Pack(new BadRequest
                    {
                        FieldViolations =
                        {
                            new BadRequest.Types.FieldViolation
                            {
                                Field = "chatId"
                            }
                        }
                    })
                }
            }.ToRpcException(),

            StatusCode.NotFound,

            "chatId"
        },
        {
            new Google.Rpc.Status
            {
                Code = (int)Code.InvalidArgument,
                Message = "Invalid Argument",
                Details =
                {
                    Any.Pack(new BadRequest
                    {
                        FieldViolations =
                        {
                            new BadRequest.Types.FieldViolation
                            {
                                Field = "chatId"
                            }
                        }
                    })
                }
            }.ToRpcException(),

            StatusCode.InvalidArgument,

            "chatId"
        }
    };
}

#endregion