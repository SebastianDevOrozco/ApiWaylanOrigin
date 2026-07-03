using API_Waylan_Origin.DTOs.ProductoDto;
using API_Waylan_Origin.Enums;

namespace API_Waylan_Origin.Interfaces.Producto
{
    public interface IProductoService
    {
        //CREAR
        Task<ProductoReadAdminDto> CrearProducto(ProductoCreateDto productoCreate);

        //LEER
        Task<IEnumerable<ProductoReadAdminDto>> ListarProductosAdmin();
        Task<IEnumerable<ProductoReadDto>> ListarProductos();
        Task<IEnumerable<ProductoReadDto>> ListarProductosTueste(Tueste tueste);
        Task<IEnumerable<ProductoReadDto>> ListarProductosProceso(Proceso proceso);

        //ACTUALIZAR
        Task<ProductoReadAdminDto> ActualizarProducto(int id, ProductoUpdateDto productoUpdate);

        //ELIMINAR
        Task EliminarProducto(int id);

        //PATCH
        Task EditarEstadoProducto(int id, bool nuevoEstado);
    }
}
