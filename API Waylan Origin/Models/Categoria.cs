namespace API_Waylan_Origin.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion {  get; set; }
        public bool Activo { get; set; }

        //Relaciones
        public ICollection<Producto> Productos { get; set; } = new HashSet<Producto>();
    }
}
