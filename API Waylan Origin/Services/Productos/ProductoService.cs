using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.ProductoDto;
using API_Waylan_Origin.Interfaces.Producto;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public ProductoService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<ProductoReadAdminDto> CrearProducto(ProductoCreateDto productoCreate)
        {
            await ValidarCategoria(productoCreate.IdCategoria);
    
            var productoNuevo = _mapper.Map<Producto>(productoCreate);

            productoNuevo.Activo = true;

            _appDbContext.Productos.Add(productoNuevo);
            await _appDbContext.SaveChangesAsync();

            //esta linea permite mapear el producto con el nombre de la categoria
            await _appDbContext.Entry(productoNuevo).Reference(p => p.Categoria).LoadAsync();

            return _mapper.Map<ProductoReadAdminDto>(productoNuevo);
        }

        public async Task<IEnumerable<ProductoReadAdminDto>> ListarProductosAdmin()
        {
            var productos = await _appDbContext.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            if(productos == null)
                return new List<ProductoReadAdminDto>();

            return _mapper.Map<IEnumerable<ProductoReadAdminDto>>(productos);
        }

        public async Task<IEnumerable<ProductoReadDto>> ListarProductos()
        {
            var productos = await _appDbContext.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo == true)
                .ToListAsync();

            if (productos == null)
                return new List<ProductoReadDto>();

            return _mapper.Map<IEnumerable<ProductoReadDto>>(productos);
        }

        public async Task<ProductoReadAdminDto> ActualizarProducto(int id, ProductoUpdateDto productoUpdate)
        {
            var producto = await ValidarExistenciaProducto(id);
           
            await ValidarCategoria(productoUpdate.IdCategoria);

            _mapper.Map(productoUpdate, producto);
            await _appDbContext.SaveChangesAsync();

            //esta linea permite mapear el producto con el nombre de la categoria
            await _appDbContext.Entry(producto).Reference(p => p.Categoria).LoadAsync();

            return _mapper.Map<ProductoReadAdminDto>(producto);
        }

        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await ValidarExistenciaProducto(id);

            producto.Activo = false;
            await _appDbContext.SaveChangesAsync();

            return true;

        }

        public async Task EditarEstadoCategoria(int id, bool nuevoEstado)
        {
            var producto = await ValidarExistenciaProducto(id);

            producto.Activo = nuevoEstado;
            await _appDbContext.SaveChangesAsync();
        }


        //METODOS SECUNDARIOS
        private async Task ValidarCategoria(int id)
        {
            var existeCategoria = await _appDbContext.categorias
                .AnyAsync(c => c.Id == id && c.Activo);

            if (!existeCategoria)
                throw new KeyNotFoundException($"La categoria con el ID {id} NO existe");

        }

        private async Task<Producto> ValidarExistenciaProducto(int id)
        {
            var producto = await _appDbContext.Productos
               .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new KeyNotFoundException($"El Producto con el ID {id} No Existe");

            return producto;
        }
    }
}
