namespace API_Waylan_Origin.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public string? Descripcion {  get; set; }
        public decimal Precio { get; set; }
        public int Stock {  get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public bool Activo { get; set; }

        //Relaciones
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new HashSet<DetallePedido>();
        public Categoria Categoria { get; set; } = null!;
    }
}
