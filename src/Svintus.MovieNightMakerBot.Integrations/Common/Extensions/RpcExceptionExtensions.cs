using Google.Rpc;
using Grpc.Core;

namespace Svintus.MovieNightMakerBot.Integrations.Common.Extensions;

internal static class RpcExceptionExtensions
{
    public static bool IsBadRequest(this RpcException exception, StatusCode? statusCode = null, string? violatedField = null)
    {
        var status = exception.GetRpcStatus();
        var badRequest = status?.GetDetail<BadRequest>();

        if (badRequest is null)
            return false;

        if (statusCode is not null && status?.Code != (int)statusCode)
            return false;

        if (violatedField is not null && badRequest.FieldViolations.All(v => v.Field != violatedField))
            return false;

        return true;
    }
}