using AutoMapper;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

	public static class TokenRequestMappers
	{
		static TokenRequestMappers()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TokenRequestMapperProfile>()).CreateMapper();
		}

		 static IMapper Mapper { get; }

		public static ClientCredentialsTokenRequest Map(this AngelTokenRequest source)
		{
			return source == null ? null : Mapper.Map<ClientCredentialsTokenRequest>(source);
		}

		public static RefreshTokenRequest MapRefRequest(this AngelTokenRequest source)
		{
			return source == null ? null : Mapper.Map<RefreshTokenRequest>(source);
		}

		public static PasswordTokenRequest MapPasswordRequest(this AngelTokenRequest source)
		{
			return source == null ? null : Mapper.Map<PasswordTokenRequest>(source);
		}

	}


	public static class TokenResponseMappers
	{
		static TokenResponseMappers()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TokenResponseMapperProfile>()).CreateMapper();
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


			CreateMap<AngelTokenRequest, PasswordTokenRequest>()
				.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
				.ForMember(dest => dest.ClientSecret, opt => opt.MapFrom(src => src.ClientSecret))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
				.ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Scopes))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

			CreateMap<AngelTokenRequest, RefreshTokenRequest>()
				.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
				.ForMember(dest => dest.ClientSecret, opt => opt.MapFrom(src => src.ClientSecret))
				.ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
				.ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src.GrantType))
				.ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Scopes));

		}
	}

	public class TokenResponseMapperProfile : Profile
	{
		public TokenResponseMapperProfile()
		{
			CreateMap<TokenResponse, AngelTokenResponse>()
				.ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.AccessToken))
				.ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
				.ForMember(dest => dest.IsError, opt => opt.MapFrom(src => src.IsError))
				.ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.HttpStatusCode.ToString()))
				.ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Error));

		}
	}


}
