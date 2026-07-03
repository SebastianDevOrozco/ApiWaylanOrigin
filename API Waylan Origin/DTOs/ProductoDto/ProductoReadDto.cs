using API_Waylan_Origin.DTOs.NotasDto;

namespace API_Waylan_Origin.DTOs.ProductoDto
{
    public class ProductoReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
        public string Tueste {  get; set; } = string.Empty;
        public string Proceso { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;

        public List<NotaReadDto> notas { get; set; } = new List<NotaReadDto>();
    }
}
