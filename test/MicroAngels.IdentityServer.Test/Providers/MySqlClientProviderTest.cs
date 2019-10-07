using MicroAngels.IdentityServer.Providers.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MicroAngels.IdentityServer.Test.Providers
{

	public class MySqlClientProviderTest
	{

		MySqlStoreOptions _options;
		string connectionString = "";
		MySqlClientProvider provider;
		string clientId = "";

		public MySqlClientProviderTest()
		{
			_options = new MySqlStoreOptions() { ConnectionStrings = connectionString };
			provider = new MySqlClientProvider(_options);
		}

		[Fact]
		public async void GetClientsByIdTest()
		{
			var client = await provider.FindClientById(clientId);

			Assert.NotNull(client);
			Assert.Equal(client.ClientId, clientId);
			Assert.NotNull(client.ClientSecrets);
			Assert.NotNull(client.AllowedGrantTypes);
			Assert.NotNull(client.AllowedScopes);
		}

	}

}
