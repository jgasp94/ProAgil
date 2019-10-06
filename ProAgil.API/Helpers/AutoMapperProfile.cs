using System.Linq;
using AutoMapper;
using ProAgil.API.Dtos;
using ProAgil.Domain.Entities;

namespace ProAgil.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Evento, EventoDTO>()
                .ForMember(dest => dest.Palestrante, 
                    opt => {
                        opt.MapFrom(src => src.PalestranteEventos.Select(x => x.Palestrante).ToList());
                    });

            CreateMap<Palestrante , PalestranteDTO>()
                .ForMember(dest => dest.Evento,
                    opt => {
                        opt.MapFrom(
                            src => src.PalestranteEventos.Select(x => x.Evento).ToList()
                        );
                    });
                    

            CreateMap<Lote, LoteDTO>();
            CreateMap<RedeSocial, RedeSocialDTO>();
        }
    }
}