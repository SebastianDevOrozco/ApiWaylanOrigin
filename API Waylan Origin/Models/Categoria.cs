namespace API_Waylan_Origin.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }

        //Relaciones
        public ICollection<Producto> Productos { get; set; }
    }
}
