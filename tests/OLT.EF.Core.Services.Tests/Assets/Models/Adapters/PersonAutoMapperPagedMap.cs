using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Adapters
{
    public class PersonAutoMapperPagedMap : OltAdapterPagedMap<PersonEntity, PersonAutoMapperPagedDto>
    {
        public override void BuildMap(IMappingExpression<PersonEntity, PersonAutoMapperPagedDto> mappingExpression)
        {
            PersonAutoMapperPagedDto.BuildMap(mappingExpression);
        }

        public override IOrderedQueryable<PersonEntity> DefaultOrderBy(IQueryable<PersonEntity> queryable)
        {
            return queryable.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst).ThenBy(p => p.Id);
        }
    }
}
