using System.ComponentModel.DataAnnotations;

namespace API_Waylan_Origin.DTOs.CategoriaDto
{
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        public string? Descripcion { get; set; }
    }
}
