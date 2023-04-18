using Microsoft.EntityFrameworkCore;

namespace Returns.Logic.Utils;

public static class QueryableExtensions
{
    public static async Task<HashSet<T>> ToHashSetAsync<T>(this IQueryable<T> queryable, IEqualityComparer<T>? comparer = null)
    {
        var collection = await queryable.ToListAsync();

        return collection.ToHashSet(comparer);
    }
}
