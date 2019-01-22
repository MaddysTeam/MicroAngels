using Business.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MessageCenter.Test
{

    public class SubscribeServiceTest : BaseTest
    {

        public SubscribeServiceTest()
        {
            var server = new TestServer
                (WebHost.CreateDefaultBuilder()
                 .UseContentRoot(GetProjectPath("MicroAngels.sln", "", typeof(Startup).Assembly))
                .UseStartup<Startup>()
                );

            _subscribeService = server.Host.Services.GetService<ISubscribeService>();
        }

        [Fact]
        public async void GetsubscribesShouldOk()
        {
            var count = 0;
            var result = await _subscribeService.GetSubscribes(null, null, null, 0, 0, out count);
            Assert.True(count > 0);
            Assert.NotEmpty(result);

            result = await _subscribeService.GetSubscribes(null, "service123", null, 0, 0, out count);
            Assert.True(count > 0);
            Assert.NotEmpty(result);

            result = await _subscribeService.GetSubscribes("Jimmy", null, null, 0, 0, out count);
            Assert.True(count > 0);
            Assert.NotEmpty(result);

            result = await _subscribeService.GetSubscribes(null, "service123", "none", 0, 0, out count);
            Assert.True(count <= 0);
            Assert.Empty(result);
        }

        private ISubscribeService _subscribeService;

    }

}
