using API_Waylan_Origin.Models;

namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class PedidoCreateDto
    {
        public ICollection<DetallePedidoCreateDto> Detalles { get; set; } = new List<DetallePedidoCreateDto>();
    }
}

