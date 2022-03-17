using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Searchers
{
    public class PersonFirstNameStartsWithSearcher : OltSearcher<PersonEntity>
    {
        public PersonFirstNameStartsWithSearcher(string startsWith)
        {
            StartsWith = startsWith;
        }

        public string StartsWith { get; }

        public override IQueryable<PersonEntity> BuildQueryable(IQueryable<PersonEntity> queryable)
        {
            return queryable.Where(p => p.NameFirst.Contains(StartsWith));
        }
    }
}
