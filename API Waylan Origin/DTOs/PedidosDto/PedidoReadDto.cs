namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class PedidoReadDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string EstadoPedido { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        public ICollection<DetallePedidoReadDto> Detalles { get; set;}
    }
}

