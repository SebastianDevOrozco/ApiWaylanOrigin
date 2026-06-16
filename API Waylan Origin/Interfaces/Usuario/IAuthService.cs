using API_Waylan_Origin.DTOs.UsuarioDto;

namespace API_Waylan_Origin.Interfaces.Usuario
{
    public interface IAuthService
    {
        Task<UsuarioReadDto> Registrar(UsuarioCreateDto usuarioCreateDto);
        Task<string?> Login(string Email, string Password);
    }
}
