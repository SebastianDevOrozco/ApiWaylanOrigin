using API_Waylan_Origin.DTOs.ProductoDto;

namespace API_Waylan_Origin.Interfaces.Producto
{
    public interface IProductoService
    {
        //CREAR
        Task<ProductoReadAdminDto> CrearProducto(ProductoCreateDto productoCreate);

        //LEER
        Task<IEnumerable<ProductoReadAdminDto>> ListarProductosAdmin();
        Task<IEnumerable<ProductoReadDto>> ListarProductos();

        //ACTUALIZAR
        Task<ProductoReadAdminDto> ActualizarProducto(int id, ProductoUpdateDto productoUpdate);

        //ELIMINAR
        Task<bool> EliminarProducto(int id);

        //PATCH
        Task EditarEstadoCategoria(int id, bool nuevoEstado);
    }
}
