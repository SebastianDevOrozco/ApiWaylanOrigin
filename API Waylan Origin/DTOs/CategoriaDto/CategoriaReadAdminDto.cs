namespace API_Waylan_Origin.DTOs.CategoriaDto
{
    public class CategoriaReadAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion {  get; set; }
        public bool Activo { get; set; }
    }
}
