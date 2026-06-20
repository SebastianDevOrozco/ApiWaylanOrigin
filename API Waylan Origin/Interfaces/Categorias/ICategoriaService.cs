using API_Waylan_Origin.DTOs.CategoriaDto;

namespace API_Waylan_Origin.Interfaces.Categoria
{
    public interface ICategoriaService
    {
        //CREAR
        Task<CategoriaReadAdminDto> CrearCategoria(CategoriaCreateDto categoriaCreateDto);

        //LEER
        Task<IEnumerable<CategoriaReadAdminDto>> ListarCategoriasAdmin();
        Task<IEnumerable<CategoriaReadDto>> ListarCategorias();

        //ACTUALIZAR
        Task<CategoriaReadAdminDto> ActualizarCategoria (int categoriaId, CategoriaUpdateDto categoriaUpdateDto);

        //DELETE
        Task<bool> EliminarCategoria(int idCategoria);

        //PATCH
        Task EditarEstadoCategoria(int categoriaId, bool nuevoEstado);
    }
}
