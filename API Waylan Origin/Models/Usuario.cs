namespace API_Waylan_Origin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; }

        //Relaciones
        public Rol Rol { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        
    }
}
