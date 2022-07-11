namespace OLT.Core
{
    public class OltGenericFilterTemplate<TEntity, TValueType> : OltGenericFilter<TEntity, TValueType>, IOltGenericFilterTemplate
        where TEntity : class, IOltEntity
    {

        private readonly IOltFilterTemplate _filterTemplate;

        public OltGenericFilterTemplate(IOltFilterTemplate<TValueType> filterTemplate, IOltEntityQueryBuilder<TEntity, TValueType> searcher) : base(filterTemplate, searcher)
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
