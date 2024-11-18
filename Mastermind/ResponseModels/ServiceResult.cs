namespace Mastermind.ResponseModels
{
    public record ServiceResult<T>(bool IsSuccess, string Error, T? body);
}
