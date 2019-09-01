using Business;
using MicroAngels.Core.Plugins;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class BaseController : AngleBaseController
	{
		public BaseController() : base()
		{
			Mapper = MicroAngels.Core.Plugins.Mapper.Create(typeof(MapperProfile));
		}

		protected readonly AutoMapper.IMapper Mapper;
	}
}
