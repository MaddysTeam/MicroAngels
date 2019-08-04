using AutoMapper;

namespace Business
{

	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<UserInfo, UserViewModel>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.RealName, opt => opt.MapFrom(src => src.RealName))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
			.ReverseMap();

			CreateMap<SystemRole, RoleViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
			.ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.SystemId, opt => opt.MapFrom(src => src.SystemId))
			.ReverseMap();

			CreateMap<Menu, MenuViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuId))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.LinkUrl, opt => opt.MapFrom(src => src.LinkUrl))
			.ForMember(dest => dest.SystemId, opt => opt.MapFrom(src => src.SystemId))
			.ReverseMap();

			CreateMap<Interface, InterfaceViewModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InterfaceId))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.Params, opt => opt.MapFrom(src => src.Parmas))
			.ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
			.ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
			.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
			.ForMember(dest => dest.IsAllowAnonymous, opt => opt.MapFrom(src => src.IsAllowAnonymous))
			.ReverseMap();

			CreateMap<Assets, AssetsViewModel>()
			.ForMember(dest => dest.id, opt => opt.MapFrom(src => src.AssetsId))
			.ForMember(dest => dest.title, opt => opt.MapFrom(src => src.AssetsName))
			.ForMember(dest => dest.parentId, opt => opt.MapFrom(src => src.ParentId))
			.ForMember(dest => dest.isbind, opt => opt.MapFrom(src => src.IsBind))
			.ForMember(dest => dest.link, opt => opt.MapFrom(src => src.Menu.LinkUrl))
			.ReverseMap();
		}
	}


	public static class Mapper
	{
		static Mapper()
		{
			InnerMappers = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()).CreateMapper();
		}

		public static T Map<S, T>(this S source) where T : class
												 where S : class
		{
			return InnerMappers.Map<T>(source);
		}

		static IMapper InnerMappers { get; }
	}

}
