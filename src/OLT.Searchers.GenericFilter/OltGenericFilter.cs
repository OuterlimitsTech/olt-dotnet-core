using System.Linq;

namespace OLT.Core
{
    public abstract class OltGenericFilter<TValueType> : IOltGenericFilter
    {        

        protected OltGenericFilter(IOltGenericParameterParser<TValueType> parser)
        {
            Parser = parser;
        }

        protected virtual IOltGenericParameterParser<TValueType> Parser { get; }

        public bool HasValue(IOltGenericParameter parameters)
        {
            return Parser.Parse(parameters);
        }
    }

    public abstract class OltGenericFilter<TEntity, TValueType> : IOltGenericFilter<TEntity>
        where TEntity : class, IOltEntity
    { 

        protected OltGenericFilter(IOltGenericParameterParser<TValueType> parser, IOltEntityQueryBuilder<TEntity, TValueType> queryBuilder)
        {
            Parser = parser;
            QueryBuilder = queryBuilder;
        }

        protected virtual IOltGenericParameterParser<TValueType> Parser { get; }
        protected virtual IOltEntityQueryBuilder<TEntity, TValueType> QueryBuilder { get; }

        public bool HasValue(IOltGenericParameter parameters)
        {
            var hasValue = Parser.Parse(parameters);
            QueryBuilder.Value = Parser.Value;
            return hasValue;
        }

        public IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable)
        {
            if (!Parser.HasValue) return queryable;
            QueryBuilder.Value = Parser.Value;
            return QueryBuilder.BuildQueryable(queryable);
        }
    }

}
