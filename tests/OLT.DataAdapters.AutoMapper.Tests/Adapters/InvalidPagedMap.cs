using AutoMapper;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.AutoMapper.Tests.Adapters
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class InvalidPagedMap : OltAdapterPagedMap<AdapterObject8, AdapterObject9>
#pragma warning restore CS0618 // Type or member is obsolete
    {

        //public InvalidMap()
        //{
        //    BuildMap(CreateMap<AdapterObject8, AdapterObject9>());
        //}

        public override void BuildMap(IMappingExpression<AdapterObject8, AdapterObject9> mappingExpression)
        {
            mappingExpression
                .ForMember(f => f.StreetAddress, opt => opt.MapFrom(t => t.Street))
                .ForMember(f => f.Name, opt => opt.MapFrom(t => t.Names.SelectMany(s => s.FullName).ToList()))  //Invalid Mapping                                                                                                                
                .ReverseMap()
                ;
        }

        public override IQueryable<AdapterObject8> DefaultOrderBy(IQueryable<AdapterObject8> queryable)
        {
            return queryable.OrderBy(p => p.Street);
        }
    }
}
