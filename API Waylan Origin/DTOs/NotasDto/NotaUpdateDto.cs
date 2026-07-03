using System.ComponentModel.DataAnnotations;

namespace API_Waylan_Origin.DTOs.NotasDto
{
    public class NotaUpdateDto
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;
    }
}
