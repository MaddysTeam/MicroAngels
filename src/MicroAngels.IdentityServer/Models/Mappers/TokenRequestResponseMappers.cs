using AutoMapper;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

	public static class CredentialTokenRequestMappers
	{
		static CredentialTokenRequestMappers()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TokenRequestMapperProfile>()).CreateMapper();
		}

		 static IMapper Mapper { get; }

		public static ClientCredentialsTokenRequest Map(this AngelTokenRequest source)
		{
			return source == null ? null : Mapper.Map<ClientCredentialsTokenRequest>(source);
		}

	}


	public static class TokenResponseMappers
	{
		static TokenResponseMappers()
		{
			//CredentialTokenMapper = new MapperConfiguration(cfg => cfg.AddProfile<TokenRequestMapperProfile>()).CreateMapper();
		}

		static IMapper Mapper { get; }

		public static AngelTokenResponse Map(this TokenResponse source)
		{
			return source == null ? null : Mapper.Map<AngelTokenResponse>(source);
		}
	}


	public class TokenRequestMapperProfile : Profile
	{
		public TokenRequestMapperProfile()
		{
			CreateMap<AngelTokenRequest, ClientCredentialsTokenRequest>()
				.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
				.ForMember(dest => dest.ClientSecret, opt => opt.MapFrom(src => src.ClientSecret))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
				.ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Scopes));

		}
	}

	public class TokenResponseMapperProfile : Profile
	{

	}
}
