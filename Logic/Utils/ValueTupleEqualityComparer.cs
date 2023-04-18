namespace Returns.Logic.Utils;

public class ValueTupleEqualityComparer<T1, T2> : IEqualityComparer<(T1, T2)>
{
    private readonly IEqualityComparer<T1> _equalityComparer1;
    private readonly IEqualityComparer<T2> _equalityComparer2;

    public ValueTupleEqualityComparer(
        IEqualityComparer<T1>? equalityComparer1 = null,
        IEqualityComparer<T2>? equalityComparer2 = null
    )
    {
        _equalityComparer1 = equalityComparer1 ?? EqualityComparer<T1>.Default;
        _equalityComparer2 = equalityComparer2 ?? EqualityComparer<T2>.Default;
    }

    public bool Equals((T1, T2) x, (T1, T2) y)
    {
        return
            _equalityComparer1.Equals(x.Item1, y.Item1) &&
            _equalityComparer2.Equals(x.Item2, y.Item2);
    }

    public int GetHashCode((T1, T2) obj)
    {
        return HashCode.Combine(
            obj.Item1 is null ? 0 : _equalityComparer1.GetHashCode(obj.Item1),
            obj.Item2 is null ? 0 : _equalityComparer2.GetHashCode(obj.Item2)
        );
    }
}
