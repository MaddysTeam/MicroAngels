using MicroAngels.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using IdentityServer4.AccessTokenValidation;

namespace MicroAngels.IdentityServer.Extensions
{

	public static class IdentityServerAuthenticationExtensions
	{

		public static IServiceCollection AddIdsAuthentication(this IServiceCollection builder, IdentityAuthenticationOptions options)
		{
			builder.AddAuthentication(options.Scheme)
				.AddIdentityServerAuthentication(opt =>
				{
					opt.Authority = options.Authority;
					opt.RequireHttpsMetadata = options.RequireHttps;
					opt.ApiSecret = options.ApiSecret;	
					opt.ApiName = options.ApiName;
					opt.SupportedTokens = SupportedTokens.Both;
				});


			return builder;
		}

	}

}
