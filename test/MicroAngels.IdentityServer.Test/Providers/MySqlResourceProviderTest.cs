using MicroAngels.IdentityServer.Providers.MySql;
using Xunit;

namespace MicroAngels.IdentityServer.Test.Providers
{

	public class MySqlResourceProviderTest
	{

		MySqlStoreOptions _options;
		string connectionString = "";
		MySqlResourceProvider provider;

		public MySqlResourceProviderTest()
		{
			_options = new MySqlStoreOptions() { ConnectionStrings = connectionString };
			provider = new MySqlResourceProvider(null, _options);

		}

		[Fact]
		public async void GetAPIResourcesTest()
		{
			var resources = await provider.FindResource(string.Empty);
			Assert.Null(resources);

			resources = await provider.FindResource("");
			Assert.Null(resources);
			Assert.NotNull(resources.Scopes);
		}

		[Fact]
		public async void GetAPIResourceByScopesTest()
		{
			string[] scopes = { "", "" };
			var resources = await provider.FindIdentityResourcesByScopeAsync(null);
			Assert.Null(resources);

			resources = await provider.FindIdentityResourcesByScopeAsync(scopes);
			Assert.Null(resources);

			scopes =new string[]{"","" };
			resources = await provider.FindIdentityResourcesByScopeAsync(scopes);
			Assert.NotNull(resources);
		}

		[Fact]
		public void GetIdentityResourceByScopesTest()
		{

		}


		[Fact]
		public void GetAllResourceTest()
		{


		}

	}

}
