using AutoMapper;

namespace Business
{

	public class MapperProfile : Profile
	{

		public MapperProfile()
		{
			CreateMap<SignupViewModel, Account>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
			.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ReverseMap();

		}

	}

}
