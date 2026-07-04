namespace API_Waylan_Origin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; }

    
        public bool EmailVerificado { get; set; } = false;
        public string? TokenVerificacion { get; set; }
        public DateTime? TokenExpiracion { get; set; }

        //Relaciones
        public Rol Rol { get; set; } = null!;
        public ICollection<Pedido> Pedidos { get; set; } = new HashSet<Pedido>();
        
    }
}
