using AutoMapper;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace MicroAngels.IdentityServer.Models
{

    public static class ClientMappers
    {

        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>()).CreateMapper();
        }

        public static Client Map(this IdentityClient source)
        {
            return Mapper.Map<Client>(source);
        }

        public static IdentityClient ReverseMap(this Client source)
        {
            return Mapper.Map<IdentityClient>(source);
        }

        static IMapper Mapper { get; }

    }


    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {
            CreateMap<IdentityClientProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<IdentityClient, Client>()
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(src => src != null))
                .ReverseMap();

            CreateMap<IdentityClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientClaim, Claim>(MemberList.None)
                .ConstructUsing(src => new Claim(src.Type, src.Value))
                .ReverseMap();

            CreateMap<IdentityClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            CreateMap<IdentityClientSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
        }
    }

}
