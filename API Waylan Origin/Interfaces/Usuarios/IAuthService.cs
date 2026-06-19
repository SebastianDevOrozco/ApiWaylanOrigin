using API_Waylan_Origin.DTOs.UsuarioDto;

namespace API_Waylan_Origin.Interfaces.Usuarios
{
    public interface IAuthService
    {
        Task<UsuarioReadDto> Registrar(UsuarioCreateDto usuarioCreateDto);
        Task<string?> Login(string Email, string Password);
    }
}
