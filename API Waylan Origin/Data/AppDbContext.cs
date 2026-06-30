using API_Waylan_Origin.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Tablas 
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Nota> Notas {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Modificacion para que los atributos ENUMS los guarde en tipo string y no en int
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Producto>()
                .Property(p => p.tueste)
                .HasConversion<string>();

            modelBuilder.Entity<Producto>()
                .Property(p => p.proceso)
                .HasConversion<string>();

            //  RELACIONES

            //Relacion Rol --> Usuario
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.IdRol);

            //Relacion Usuario --> Pedido
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p =>  p.Usuario)
                .HasForeignKey(p => p.IdUsuario);

            //Relacion Pedido --> DetallePedido
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.DetallesPedido)
                .WithOne(d =>  d.Pedido)
                .HasForeignKey(d => d.IdPedido);

            //Relacion Producto --> DetallePedido
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.DetallesPedido)
                .WithOne(d => d.Producto)
                .HasForeignKey(d => d.IdProducto);

            //Relacion Categoria --> Productos
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.IdCategoria);

            //Relacion muchos a muchos Producto --> Notas
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Notas)
                .WithMany(n => n.Productos)
                .UsingEntity(m => m.ToTable("ProductoNotas"));
                
        }

    }
}
