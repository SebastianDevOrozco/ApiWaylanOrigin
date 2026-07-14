using API_Waylan_Origin.Models;
using System.ComponentModel.DataAnnotations;

namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class PedidoCreateDto
    {
        [Required(ErrorMessage = "La Direccion es obligatoria")]
        public string Direccion { get; set; } = string.Empty;
        public ICollection<DetallePedidoCreateDto> Detalles { get; set; } = new List<DetallePedidoCreateDto>();
    }
}

