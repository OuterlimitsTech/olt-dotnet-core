using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Adapters
{
    public class PersonModelAdapter : OltAdapterQueryable<PersonEntity, PersonModel>
    {
        public PersonModelAdapter()
        {
            this.WithOrderBy(o => o.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst).ThenBy(p => p.Id));
        }

        public override void Map(PersonEntity source, PersonModel destination)
        {
            destination.PersonId = source.Id;
            destination.UniqueId = source.UniqueId;
            destination.Name.First = source.NameFirst;
            destination.Name.Middle = source.NameMiddle;
            destination.Name.Last = source.NameLast;
        }

        public override void Map(PersonModel source, PersonEntity destination)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(source.Name.First);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(source.Name.Last);
            destination.NameFirst = source.Name.First;
            destination.NameMiddle = source.Name.Middle;
            destination.NameLast = source.Name.Last;

            if (source.UniqueId.HasValue)
            {
                destination.UniqueId = source.UniqueId.Value;
            }
        }

        public override IQueryable<PersonModel> Map(IQueryable<PersonEntity> queryable)
        {
            return queryable.Select(entity => new PersonModel
            {
                PersonId = entity.Id,
                UniqueId = entity.UniqueId,
                Name = new NameModel
                {
                    First = entity.NameFirst,
                    Middle = entity.NameMiddle,
                    Last = entity.NameLast
                }
            });
        }
    }

}
