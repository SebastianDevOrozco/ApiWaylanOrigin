using API_Waylan_Origin.Enums;

namespace API_Waylan_Origin.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public Tueste tueste { get; set; }
        public Proceso proceso { get; set; }
        public string? Descripcion {  get; set; }
        public decimal Precio { get; set; }
        public int Stock {  get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public bool Activo { get; set; }

        //Relaciones
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new HashSet<DetallePedido>();
        public ICollection<Nota> Notas { get; set; } = new HashSet<Nota>();
        public Categoria Categoria { get; set; } = null!;
    }
}
