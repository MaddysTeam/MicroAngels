using MicroAngels.Core.Test.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace MicroAngels.Core.Test.Extensions
{

	public class ServiceProviderExtensionTest
	{

		ConfigurationBuilder _builder;
		IConfiguration _configuration;
		IConfiguration _anotherConfiguration;

		public ServiceProviderExtensionTest()
		{
			_builder = new ConfigurationBuilder();
			_configuration = _builder.Build();
			_configuration.Bind("Options", new Options());
			var temp = _configuration.GetValue<Options>("Options");
		}

		[Fact]
		public void GetMonitorTest()
		{
			var services = new ServiceCollection();
			var provider = services.BuildServiceProvider();
			var monitor = provider.GetMonitor<Options>((O, t) => { });

			Assert.NotNull(monitor);
		}

		[Fact]
		public void OptionMonitorTest()
		{
			var services = new ServiceCollection();
			var provider = services.AddOptions()
					.Configure<Options>(_configuration)
					.BuildServiceProvider();

			var opt = provider.GetService<Options>();

			var monitor = provider.GetMonitor<Options>((O, t) =>
			{
				Assert.True(O.Option1 == "Hello world");
			});

			_anotherConfiguration = _configuration.GetSection("Options");
		}

	}

}
