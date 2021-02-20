using CSharpFunctionalExtensions;

namespace Kubera.Application.Common.Extensions
{
    public static class ResultEx
    {
        public static IResult<T> AsResult<T>(this T source) => Result.Success(source);
    }
}
