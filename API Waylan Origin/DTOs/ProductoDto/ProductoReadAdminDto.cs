namespace API_Waylan_Origin.DTOs.ProductoDto
{
    public class ProductoReadAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; }
        public bool Activo { get; set; }
    }
}
