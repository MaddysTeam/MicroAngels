using AutoMapper;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<ApiResourceMapperProfile>()).CreateMapper();
        }

        static IMapper Mapper { get; }

        public static ApiResource Map(this IdentityApiResource source)
        {
            return source == null ? null : Mapper.Map<ApiResource>(source);
        }

        public static IdentityApiResource ReverseMap(this ApiResource source)
        {
            return source == null ? null : Mapper.Map<IdentityApiResource>(source);
        }
    }


    public class ApiResourceMapperProfile : Profile
    {

        public ApiResourceMapperProfile()
        {
            CreateMap<IdentityApiResource, ApiResource>()
                 .ConstructUsing(src => new ApiResource())
                 .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                 .ReverseMap();

            CreateMap<IdentityApiResourceClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<IdentityApiSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<IdentityApiScope, Scope>(MemberList.Destination)
                .ConstructUsing(src => new Scope())
                .ReverseMap();

            CreateMap<IdentityApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }

    }

}
