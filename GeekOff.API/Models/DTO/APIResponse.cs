namespace GeekOff.Models;

// legacy method pending refactor
public class ApiResponse
{
    public bool SuccessInd { get; set; }
    public string? Response { get; set; }
}

// generic object method
public class ApiResponse<T>: IResult<T, QueryStatus>
{
    public T? Value { get; }
    public QueryStatus Status { get; }

    public ApiResponse(QueryStatus status)
    {
        Status = status;
    }

    private ApiResponse(T? value, QueryStatus status)
    {
        Value = value;
        Status = status;
    }

    public static ApiResponse<T> Success(T? val = default)
        => new(val, QueryStatus.Success);

    public static ApiResponse<T> NotFound(T? val = default)
        => new(val, QueryStatus.NotFound);

    public static ApiResponse<T> Conflict(T? val = default)
        => new(val, QueryStatus.Conflict);

    public static ApiResponse<T> Forbidden()
        => new(default, QueryStatus.Forbidden);

    public static ApiResponse<T> NoContent()
        => new(default, QueryStatus.NoContent);

    public static ApiResponse<T> BadRequest()
        => new(default, QueryStatus.BadRequest);
}
