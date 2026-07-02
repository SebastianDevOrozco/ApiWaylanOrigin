using System.ComponentModel.DataAnnotations;

namespace API_Waylan_Origin.DTOs.ProductoDto
{
    public class ProductoUpdateDto
    {

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Id de la categoria es obligatorio")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El Id de la nota es obligatorio")]
        public List<int> IdNotas { get; set; } = new List<int>();

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El Precio es obligatorio")]
        [Range(0.01, 1000000, ErrorMessage = "El Precio debe estar entre 0.01 y 1.000.000")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, 1000000)]
        public int Stock { get; set; }
        public IFormFile? Imagen { get; set; }

    }
}
