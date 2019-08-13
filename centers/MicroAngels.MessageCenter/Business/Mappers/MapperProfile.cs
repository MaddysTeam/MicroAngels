﻿using AutoMapper;

namespace Business
{

	public class MapperProfile : Profile
	{

		public MapperProfile()
		{
			CreateMap<Topic, TopicViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
			.ReverseMap();
		}

	}

}