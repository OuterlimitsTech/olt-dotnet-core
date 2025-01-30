using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.Core.Common.Tests.Assets;

public class PersonNameLastStartsWithSearcher : OltSearcher<PersonEntity>
{
    public PersonNameLastStartsWithSearcher(string startsWith)
    {
        StartsWith = startsWith;
    }

    public string StartsWith { get; }

    public override IQueryable<PersonEntity> BuildQueryable(IQueryable<PersonEntity> queryable)
    {
        return queryable.Where(p => p.NameLast.StartsWith(StartsWith));
    }
}