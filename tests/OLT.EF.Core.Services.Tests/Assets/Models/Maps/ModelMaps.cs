using AutoMapper;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets.Models.Maps
{
    public class ModelMaps : Profile
    {
        public ModelMaps()
        {
            PersonAutoMapperModel.BuildMap(CreateMap<PersonEntity, PersonAutoMapperModel>()).WithOrderBy(p => p.OrderBy(o => o.NameLast).ThenBy(o => o.NameFirst));
            NameAutoMapperModel.BuildMap(CreateMap<PersonEntity, NameAutoMapperModel>()).WithOrderBy(p => p.OrderBy(o => o.NameLast).ThenBy(o => o.NameFirst));
            NameAutoMapperModel.BuildMap(CreateMap<UserEntity, NameAutoMapperModel>()).AfterMap(new NameAutoMapperModelAfterMap());
        }
    }
}
