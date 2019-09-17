using Com.Ctrip.Framework.Apollo;
using MicroAngels.Core;
using Microsoft.Extensions.Configuration;

namespace MicroAngels.Configuration.Apollo
{

	public static class ApolloExtensions
	{

		public static IConfigurationBuilder AddApolloConfiguration(this IConfigurationBuilder builder,string sectionKey, params string[] namespaces)
		{
			if(sectionKey.IsNullOrEmpty())
				throw new AngleExceptions("sectionKey cannot be null");

			builder = builder.AddApollo(builder.Build().GetSection(sectionKey))
				.AddDefault()
				.BindNameSpace(namespaces);

			return builder;
		}


		public static IConfigurationBuilder AddApolloConfiguration(this IConfigurationBuilder builder, ApolloOptions options)
		{
			if (options.IsNull())
			{
				throw new AngleExceptions("apollo options value cannot be null");
			}

			builder=builder.AddApollo(options)
				.AddDefault()
				.BindNameSpace(options.Namespace);

			return builder;
		}


		public static IConfigurationBuilder AddApolloConfiguration(this IConfigurationBuilder builder, IConfiguration configuration,params string[] namespaces)
		{
			if (configuration.IsNull())
			{
				throw new AngleExceptions("configuration value cannot be null");
			}

			 builder = builder.AddApollo(configuration)
				.AddDefault()
				.BindNameSpace(namespaces);

			return builder;
		}


		private static IConfigurationBuilder BindNameSpace(this IApolloConfigurationBuilder builder,string[] namespaces)
		{
			if (namespaces.IsNull()) return builder;

			foreach (var @namespace in namespaces)
			{
				builder.AddNamespace(@namespace);
			}

			return builder;
		}

	}

}
