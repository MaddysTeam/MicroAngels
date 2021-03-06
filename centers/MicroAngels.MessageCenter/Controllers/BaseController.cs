﻿using Business;
using MicroAngels.Core;
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

		protected TokenViewModel CurrentToken => new TokenViewModel
		{
			accessToken = Request.Headers[CoreKeys.AccessToken].ToString(),
			refreshToken = Request.Headers[CoreKeys.RefreshToken].ToString()
		};
	}
}
