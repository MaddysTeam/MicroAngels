using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using IdentityServer4.Models;

namespace MicroAngels.IdentityServer.Models
{

    public static class ResistedGrantMappers
    {

        static ResistedGrantMappers()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<IdentityResourceMapperProfile>()).CreateMapper();
        }

        public static PersistedGrant Map(this IdentityPersistedGrant source)
        {
            return source == null ? null : Mapper.Map<PersistedGrant>(source);
        }

        public static IdentityPersistedGrant ReverseMap(this PersistedGrant source)
        {
            return source == null ? null : Mapper.Map<IdentityPersistedGrant>(source);
        }

        static IMapper Mapper;

    }

    public class ResistedGrantMappersProfile : Profile
    {
        public ResistedGrantMappersProfile()
        {
            CreateMap<IdentityPersistedGrant, PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }

}
