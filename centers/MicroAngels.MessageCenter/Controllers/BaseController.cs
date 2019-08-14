using MicroAngels.Core;
using Business;
using Microsoft.AspNetCore.Mvc;
using MicroAngels.Core.Plugins;

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
