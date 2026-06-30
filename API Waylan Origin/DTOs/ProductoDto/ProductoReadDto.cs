namespace API_Waylan_Origin.DTOs.ProductoDto
{
    public class ProductoReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
        public string TuesteNombre {  get; set; } = string.Empty;
        public string ProcesoNombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;

        public List<NotaReadDto> notas { get; set; } = new List<NotaReadDto>();
    }
}
