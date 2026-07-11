using API_Waylan_Origin.Enums;

namespace API_Waylan_Origin.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string CodigoSeguimiento { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }
        public EstadoPedido Estado { get; set; } 
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        // Atributos que se relacionan a WOMPI

        // Nos dirá si el pedido ya se pagó o no. Por defecto nace como "Pendiente".
        public string EstadoPago { get; set; } = "Pendiente";

        // Guardará el código único de Wompi (ej. "01-12345"). 
        // Es nuleable (?) porque al crear el pedido aún no existe este código.
        public string? WompiTransactionId { get; set; }

        // Guardará la fecha y hora exacta en la que el dinero entró.
        // También es nuleable (?) porque al inicio no hay pago.
        public DateTime? FechaPago { get; set; }

        //Relaciones
        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new HashSet<DetallePedido>();
    }
}
