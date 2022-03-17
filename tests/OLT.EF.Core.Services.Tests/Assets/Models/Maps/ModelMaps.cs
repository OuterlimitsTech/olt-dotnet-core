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
            PersonAutoMapperModel.BuildMap(CreateMap<PersonEntity, PersonAutoMapperModel>()).WithOrderBy(p => p.OrderBy(o => o.Name.Last).ThenBy(o => o.Name.First));
            NameAutoMapperModel.BuildMap(CreateMap<PersonEntity, NameAutoMapperModel>()).WithOrderBy(p => p.OrderBy(o => o.Last).ThenBy(o => o.First));
            NameAutoMapperModel.BuildMap(CreateMap<UserEntity, NameAutoMapperModel>()).AfterMap(new NameAutoMapperModelAfterMap());
        }
    }
}
