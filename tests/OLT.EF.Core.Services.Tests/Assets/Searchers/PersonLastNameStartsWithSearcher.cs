using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Searchers
{
    public class PersonLastNameStartsWithSearcher : OltSearcher<PersonEntity>
    {
        public PersonLastNameStartsWithSearcher(string startsWith)
        {
            StartsWith = startsWith;
        }

        public string StartsWith { get; }

        public override IQueryable<PersonEntity> BuildQueryable(IQueryable<PersonEntity> queryable)
        {
            return queryable.Where(p => p.NameLast.Contains(StartsWith));
        }
    }
}
