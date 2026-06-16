using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Models;
using AutoMapper;

namespace API_Waylan_Origin.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //USUARIO//

            //Mapeo de UsuarioCreateDto --> Usuario
            CreateMap<UsuarioCreateDto, Usuario>();
            //Mapeo de Usuario --> UsuarioReadDto
            CreateMap<Usuario, UsuarioReadDto>()
                .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(src => src.Rol.Nombre));
        }
    }
}
