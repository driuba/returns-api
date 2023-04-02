namespace Returns.Domain.Logic;

public class ValueResponse<T> : Response
{
    public T? Value { get; set; }
}
