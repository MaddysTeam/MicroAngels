using Com.Ctrip.Framework.Apollo;
using Microsoft.Extensions.Configuration;

namespace MicroAngels.Configuration.Apollo
{

	public static class ApolloExtensions
	{

		public static IConfigurationBuilder AddApolloConfiguration(this IConfigurationBuilder builder, ApolloOptions options)
		{
			var apolloBuilder=builder.AddApollo(options)
				.AddDefault();

			foreach (var @namespace in options.Namespace)
			{
				apolloBuilder.AddNamespace(@namespace);
			}

			return builder;
		}

	}

}
