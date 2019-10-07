using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MicroAngels.IAccountService.Test
{

	public class AccountServiceTest : BaseTest
	{

		public AccountServiceTest() : base()
		{
			_service = Server.Host.Services.GetService<Business.IAccountService>();
		}

		[Fact]
		public void SinginTest()
		{

		}

		[Fact]
		public void SignupTest()
		{

		}

		[Fact]
		public void RefreshTest()
		{

		}

		[Fact]
		public void ChangePasswordTest()
		{

		}



		private readonly Business.IAccountService _service;

	}

}
