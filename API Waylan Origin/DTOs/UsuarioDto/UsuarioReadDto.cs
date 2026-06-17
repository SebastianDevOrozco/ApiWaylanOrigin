namespace API_Waylan_Origin.DTOs.UsuarioDto
{
    public class UsuarioReadDto
    {
        public int Id { get; set; }
        public string RolNombre {  get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
}
