namespace ProductCatalog.Domain;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();
        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException();
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Fail(string message) => new Result(false, message);

    public static Result<T> Fail<T>(string message) => new Result<T>(default, false, message);

    public static Result Ok() => new Result(true, string.Empty);

    public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}