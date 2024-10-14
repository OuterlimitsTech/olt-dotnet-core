namespace OLT.Core
{
    public class OltPagedSearchJson<TModel, TCriteria> : OltPagedJson<TModel>
        where TModel : class
        where TCriteria: class
    {
        public virtual string? Key { get; set; }
        public virtual TCriteria? Criteria { get; set; }
    }
}