namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class PedidoReadAdminDto
    {
        public int Id { get; set; }
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string EmailUsuario {  get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
        public string EstadoPago { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; }

        public ICollection<DetallePedidoReadDto> DetallesAdmin { get; set; } = new List<DetallePedidoReadDto>();
    }
}
