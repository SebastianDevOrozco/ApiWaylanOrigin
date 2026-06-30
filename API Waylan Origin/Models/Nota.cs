using API_Waylan_Origin.Services.Productos;

namespace API_Waylan_Origin.Models
{
    public class Nota
    {
        public int id {  get; set; }
        public string Nombre { get; set; } = string.Empty;

        //Relaciones
        public ICollection<Producto> Productos { get; set; } = new HashSet<Producto>();
    }
}
