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
                cfg.CreateMap<ImageData, ImageDb>();
                cfg.CreateMap<ImageDb, ImageData>();
                cfg.CreateMap<PersonDb, PersonTableDb>()
                    .ForMember(dest => dest.Gender,
                        opt => opt.MapFrom(src => src.Gender.ToString()));
                cfg.CreateMap<PersonTableDb, PersonDb>()
                    .ForMember(dest => dest.Gender,
                        opt => opt.MapFrom(src => src.Gender == "Male" ? Gender.Male : Gender.Female ));
            }));

            serviceRegistry.RegisterInstance(mapper);
        }
    }
}
