using AutoMapper;
using System;

namespace MicroAngels.Core.Plugins
{

	public static class Mapper
	{

		//public static T Map<S, T>(this IMapper mapper, S source) where T : class
		//										 where S : class
		//{
		//	return mapper.Map<T>(source);
		//}


		public static IMapper Create(Type profileType)
		{
			return new MapperConfiguration(cfg => cfg.AddProfile(profileType)).CreateMapper();
		}

	}

}
