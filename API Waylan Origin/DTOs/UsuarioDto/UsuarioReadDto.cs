namespace API_Waylan_Origin.DTOs.UsuarioDto
{
    public class UsuarioReadDto
    {
        public int Id { get; set; }
        public string RolNombre {  get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
}
