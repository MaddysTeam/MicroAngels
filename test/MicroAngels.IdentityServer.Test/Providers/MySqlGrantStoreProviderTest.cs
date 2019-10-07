using MicroAngels.IdentityServer.Providers.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MicroAngels.IdentityServer.Test.Providers
{

	public class MySqlGrantStoreProviderTest
	{

		MySqlStoreOptions _options;
		string connectionString = "";
		MySqlGrantStoreProvider provider;

		public MySqlGrantStoreProviderTest()
		{
			_options = new MySqlStoreOptions() { ConnectionStrings = connectionString };
			provider = new MySqlGrantStoreProvider(null, _options);
		}
	}

}
