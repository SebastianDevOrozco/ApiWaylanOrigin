using API_Waylan_Origin.DTOs.UsuarioDto;

namespace API_Waylan_Origin.Interfaces.Usuarios
{
    public interface IUsuarioService
    {
        //LEER
        Task<UsuarioReadDto> InfoUsuario(int usuarioId);
        Task<IEnumerable<UsuarioReadDto>> ListaUsuarios();

        //DELETE
        Task<bool> DeleteUsuario(int usuarioId);

        //PATCH
        Task EditarEstadoUsuario(int usuarioId, bool nuevoEstado);

    }
}
