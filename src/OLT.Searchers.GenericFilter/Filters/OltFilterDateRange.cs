namespace OLT.Core
{

    public class OltFilterDateRange<TEntity> : OltGenericFilterTemplate<TEntity, OltDateRange>, IOltGenericFilterTemplate
       where TEntity : class, IOltEntity
    {

        public OltFilterDateRange(OltFilterTemplateDateRange filterTemplate, OltSearcherDateRange<TEntity> searcher) : base(filterTemplate, searcher)
        {

        }        
    }
}
