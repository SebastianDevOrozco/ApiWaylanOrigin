namespace API_Waylan_Origin.DTOs.UsuarioDto
{
    public class UsuarioLoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
