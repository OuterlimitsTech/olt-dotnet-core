using OLT.Core;
using OLT.EF.Common.Tests.Assets.Models;
using System.Linq;

namespace OLT.EF.Common.Tests.Assets
{
    public class PersonFirstNameStartsWithSearcher : OltSearcher<EntityPersonModel>
    {
        private bool? _includeDeleted;

        public PersonFirstNameStartsWithSearcher(string startsWith)
        {
            StartsWith = startsWith;
        }

        public PersonFirstNameStartsWithSearcher(string startsWith, bool includeDeleted)
        {
            StartsWith = startsWith;
            _includeDeleted = includeDeleted;
        }

        public string StartsWith { get; }

        public override bool IncludeDeleted => _includeDeleted ?? base.IncludeDeleted;

        public override IQueryable<EntityPersonModel> BuildQueryable(IQueryable<EntityPersonModel> queryable)
        {
            return queryable.Where(p => p.FirstName.StartsWith(StartsWith));
        }
    }


    public class PersonLastNameStartsWithSearcher : OltSearcher<EntityPersonModel>
    {
        public PersonLastNameStartsWithSearcher(string startsWith)
        {
            StartsWith = startsWith;
        }

        public string StartsWith { get; }

        public override IQueryable<EntityPersonModel> BuildQueryable(IQueryable<EntityPersonModel> queryable)
        {
            return queryable.Where(p => p.LastName.StartsWith(StartsWith));
        }
    }

}
