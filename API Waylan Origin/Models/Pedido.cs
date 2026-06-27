namespace API_Waylan_Origin.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }
        public string EstadoPedido { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        //Relaciones
        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new HashSet<DetallePedido>();
    }
}
