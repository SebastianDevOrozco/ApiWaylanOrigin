using API_Waylan_Origin.Controllers;
using API_Waylan_Origin.DTOs.CategoriaDto;
using API_Waylan_Origin.DTOs.PedidosDto;
using API_Waylan_Origin.DTOs.ProductoDto;
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

            //CATEGORIA//
            //Mapeo de CategoriaCreateDto --> Categoria
            CreateMap<CategoriaCreateDto, Categoria>();
            //Mapeo de Categoria --> CategoriaReadDto
            CreateMap<Categoria, CategoriaReadDto>();
            //Mapeo de Categoria --> CategoriaReadAdminDto
            CreateMap<Categoria, CategoriaReadAdminDto>();
            //Mapeo de CategoriaUpdateDto --> Categoria
            CreateMap<CategoriaUpdateDto, Categoria>();

            //PRODUCTO//
            CreateMap<ProductoCreateDto, Producto>();
            CreateMap<Producto, ProductoReadDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre));
            CreateMap<Producto, ProductoReadAdminDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre));
            CreateMap<ProductoUpdateDto, Producto>();

            //PEDIDOS//
            CreateMap<PedidoCreateDto, Pedido>()
               .ForMember(dest => dest.DetallesPedido, opt => opt.MapFrom(src => src.Detalles));
            CreateMap<Pedido, PedidoReadDto>()
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetallesPedido));
            CreateMap<Pedido, PedidoReadAdminDto>()
                .ForMember(dest => dest.DetallesAdmin, opt => opt.MapFrom(src => src.DetallesPedido))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => src.Usuario.Email));

            CreateMap<DetallePedidoCreateDto, DetallePedido>();
            CreateMap<DetallePedido, DetallePedidoReadDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.ImagenProducto, opt => opt.MapFrom(src => src.Producto.ImagenUrl));



        }
    }
}
