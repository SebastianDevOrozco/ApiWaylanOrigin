using API_Waylan_Origin.DTOs.UsuarioDto;

namespace API_Waylan_Origin.Interfaces.Usuario
{
    public interface IPerfilUsuarioService
    {
        Task<UsuarioReadDto> InfoUsuario(int usuarioId);
    }
}
