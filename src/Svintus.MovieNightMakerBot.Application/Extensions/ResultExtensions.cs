using Svintus.MovieNightMakerBot.Integrations.Common.Models.Results;

namespace Svintus.MovieNightMakerBot.Application.Extensions;

internal static class ResultExtensions
{
    public static Result<TValue, T> MapError<T, TValue, TError>(this Result<TValue, TError> result, Func<TError, T> mapper)
    {
        return result.IsSuccess
            ? new Result<TValue, T>(result.Value)
            : new Result<TValue, T>(mapper(result.Error));
    }
}