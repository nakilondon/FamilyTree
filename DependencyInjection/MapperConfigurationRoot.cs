using AutoMapper;
using LightInject;
using ReactNet.Models;
using ReactNet.Repositories;

namespace ReactNet.DependencyInjection
{
    public class MapperConfigurationRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonOldDb, ListPerson>()
                    .ForMember(dest => dest.Label,
                        opt => opt.MapFrom(src => src.PreferredName + " " + src.Description));
                cfg.CreateMap<PersonOldDb, FamilyTreePerson>()
                    .ForMember(dest => dest.Label,
                        opt => opt.MapFrom(src => src.PreferredName))
                    .ForMember(dest => dest.Title,
                        opt => opt.MapFrom(src => src.PreferredName))
                    .ForMember(dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.Date));
                cfg.CreateMap<PersonOldDb, PersonDetails>()
                    .ForMember(dest => dest.PreferredName,
                        opt => opt.MapFrom(src => src.PreferredName));
                cfg.CreateMap<PersonDetails, PersonOverride>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null && (string)srcMember != ""));
                cfg.CreateMap<PersonOverride, PersonDetails>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null && (string)srcMember != ""));
                cfg.CreateMap<ImageData, ImageDb>();
                cfg.CreateMap<ImageDb, ImageData>();
            }));

            serviceRegistry.RegisterInstance(mapper);
        }
    }
}
