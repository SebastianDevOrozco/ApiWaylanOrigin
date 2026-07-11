namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class PedidoReadDto
    {
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
        public string EstadoPago { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; } 


        public ICollection<DetallePedidoReadDto> Detalles { get; set;} = new List<DetallePedidoReadDto>();
    }
}

