using API_Waylan_Origin.DTOs.NotasDto;

namespace API_Waylan_Origin.DTOs.ProductoDto
{
    public class ProductoReadAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string Tueste { get; set; } = string.Empty;
        public string Proceso { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public bool Activo { get; set; }

        public List<NotaReadDto> notas { get; set; } = new List<NotaReadDto>();
    }
}
