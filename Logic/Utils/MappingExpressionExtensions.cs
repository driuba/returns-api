using AutoMapper;

namespace Returns.Logic.Utils;

public static class MappingExpressionExtensions
{
    public static IMappingExpression<TSource, TDestination> ExplicitExpansion<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
    {
        mappingExpression.ForAllMembers(c =>
        {
            c.ExplicitExpansion();
        });

        return mappingExpression;
    }
}
