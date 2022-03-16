using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Adapters
{
    public class PersonDtoAdapter : OltAdapterPaged<PersonEntity, PersonDto>
    {
        public PersonDtoAdapter()
        {

        }

        public override void Map(PersonEntity source, PersonDto destination)
        {
            destination.PersonId = source.Id;
            destination.UniqueId = source.UniqueId;
            destination.First = source.NameFirst;
            destination.Middle = source.NameMiddle;
            destination.Last = source.NameLast;
        }

        public override void Map(PersonDto source, PersonEntity destination)
        {
            destination.NameFirst = source.First;
            destination.NameMiddle = source.Middle;
            destination.NameLast = source.Last;

            if (source.UniqueId.HasValue)
            {
                destination.UniqueId = source.UniqueId.Value;
            }
        }

        public override IQueryable<PersonDto> Map(IQueryable<PersonEntity> queryable)
        {
            return queryable.Select(entity => new PersonDto
            {
                PersonId = entity.Id,
                UniqueId = entity.UniqueId,
                First = entity.NameFirst,
                Middle = entity.NameMiddle,
                Last = entity.NameLast
            });
        }

        public override IOrderedQueryable<PersonEntity> DefaultOrderBy(IQueryable<PersonEntity> queryable)
        {
            return queryable.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst).ThenBy(p => p.Id);
        }

    }
}
