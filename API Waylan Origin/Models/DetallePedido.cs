namespace API_Waylan_Origin.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        //Relaciones
        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}
