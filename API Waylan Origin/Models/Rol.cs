namespace API_Waylan_Origin.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        //Relaciones
        public ICollection<Usuario> Usuarios { get; set; } = new HashSet<Usuario>();
    }
}
