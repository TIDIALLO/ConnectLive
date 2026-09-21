namespace ConnectLive.Portal.Shared;
#nullable disable
public class Result<T> : IResult<T>
{
    #region Properties
    public bool Successed { get; set; }

    public T Data { get; set; }

    public List<string> Messages { get; set; }
    #endregion

    #region Methods
    public static Result<T> Fail(List<string> message)
    {
        return new Result<T> { Successed = false, Messages = message };
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T> { Successed = false, Messages = [message] };
    }

    public static Task<Result<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public static Task<Result<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Result<T> Success()
    {
        return new Result<T> { Successed = true};
    }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Successed = true, Data = data };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Successed = true, Data = data, Messages = [message] };
    }

    public static Task<Result<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Result<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
    #endregion
}


public interface IResult<out T>
{
    bool Successed { get; set; }
    public T Data { get; }

    public List<string> Messages { get; set; }
}