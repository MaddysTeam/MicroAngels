using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using IdentityServer4.Models;
using ids4 = IdentityServer4.Models;

namespace MicroAngels.IdentityServer.Models
{

    public static class IdentityResourceMappers
    {

        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<IdentityResourceMapperProfile>()).CreateMapper();
        }

        public static ids4.IdentityResource Map(this IdentityResource source)
        {
            return source == null ? null : Mapper.Map<ids4.IdentityResource>(source);
        }

        public static IdentityResource ReverseMap(this ids4.IdentityResource source)
        {
            return source == null ? null : Mapper.Map<IdentityResource>(source);
        }

        static IMapper Mapper;

    }

    public class IdentityResourceMapperProfile : Profile
    {
        public IdentityResourceMapperProfile()
        {
            CreateMap<IdentityResource, Resource>()
                .ConstructUsing(src=>new ids4.IdentityResource())
                .ReverseMap();

            CreateMap<IdentityClaim, string>()
              .ConstructUsing(x => x.Type)
              .ReverseMap()
              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }

}
