using AutoMapper;

namespace Returns.Api.Utils;

internal static class MappingExpressionExtensions
{
    internal static IMappingExpression<TSource, TDestination> ExplicitExpansion<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
    {
        mappingExpression.ForAllMembers(c =>
        {
            c.ExplicitExpansion();
        });

        return mappingExpression;
    }
}
