namespace OLT.Core
{
    public class OltFilterDateRange<TEntity> : OltGenericFilter<TEntity, OltDateRange>, IOltGenericFilterTemplate
       where TEntity : class, IOltEntity
    {

        private readonly OltFilterTemplateDateRange _filterTemplate;

        public OltFilterDateRange(OltFilterTemplateDateRange filterTemplate, OltSearcherDateRange<TEntity> searcher) : base(filterTemplate, searcher)
        {
            _filterTemplate = filterTemplate;
        }

        public IOltFilterTemplate FilterTemplate => _filterTemplate;
    }
}
