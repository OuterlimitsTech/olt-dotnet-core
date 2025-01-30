using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.Core.Common.Tests.Assets;

public class PersonNameFirstStartsWithSearcher : OltSearcher<PersonEntity>
{
    private bool? _includeDeleted;

    public PersonNameFirstStartsWithSearcher(string startsWith)
    {
        StartsWith = startsWith;
    }

    public PersonNameFirstStartsWithSearcher(string startsWith, bool includeDeleted)
    {
        StartsWith = startsWith;
        _includeDeleted = includeDeleted;
    }

    public string StartsWith { get; }

    public override bool IncludeDeleted => _includeDeleted ?? base.IncludeDeleted;

    public override IQueryable<PersonEntity> BuildQueryable(IQueryable<PersonEntity> queryable)
    {
        return queryable.Where(p => p.NameFirst.StartsWith(StartsWith));
    }
}
