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
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime))
			.ReverseMap();

			CreateMap<Message, MessageViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.TopicId))
			.ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
			.ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
			.ForMember(dest => dest.SendTime, opt => opt.MapFrom(src => src.SendTime))
			.ReverseMap();

			CreateMap<UserMessage, UserMessageViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
			.ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
			.ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.MessageId))
			.ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Message.Body));

			CreateMap<Subscribe, SubscribeViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.TopicId, opt => opt.MapFrom(src => src.TopicId))
			.ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
			.ForMember(dest => dest.SubscriberId, opt => opt.MapFrom(src => src.SubscriberId))
			.ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.TargetId))
			.ForMember(dest => dest.Target, opt => opt.MapFrom(src => src.Target))
			.ForMember(dest => dest.Subsriber, opt => opt.MapFrom(src => src.Subscriber))
			.ReverseMap();
		}

	}

}
