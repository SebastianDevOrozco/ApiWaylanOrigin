namespace API_Waylan_Origin.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }
        public string EstadoPedido { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        //Relaciones
        public Usuario Usuario { get; set; } 
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
