namespace API_Waylan_Origin.DTOs.PedidosDto
{
    public class DetallePedidoReadDto
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string ImagenProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal => Cantidad * PrecioUnitario;

    }
}
