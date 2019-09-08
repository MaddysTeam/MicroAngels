using Business;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class BaseController : ControllerBase
	{
		public BaseController()
		{
			Mapper = MicroAngels.Core.Plugins.Mapper.Create(typeof(MapperProfile));
		}

		protected readonly AutoMapper.IMapper Mapper;

	}
}
