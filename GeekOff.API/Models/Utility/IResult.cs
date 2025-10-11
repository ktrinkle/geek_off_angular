namespace GeekOff.Models;

public interface IResult<T, TU>
{
    T? Value { get; }
    TU Status { get; }
}
