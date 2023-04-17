namespace Returns.Domain.Dto;

public class ValueResponse<T> : Response
{
    public T? Value { get; set; }
}
