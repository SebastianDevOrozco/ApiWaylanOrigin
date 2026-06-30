using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.CategoriaDto;
using API_Waylan_Origin.Interfaces.Categoria;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services.Categorias
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public CategoriaService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<CategoriaReadAdminDto> CrearCategoria(CategoriaCreateDto categoriaCreateDto)
        {
            var nuevaCategoria = _mapper.Map<Categoria>(categoriaCreateDto);

            nuevaCategoria.Activo = true;

            _appDbContext.categorias.Add(nuevaCategoria);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaReadAdminDto>(nuevaCategoria);
        }

       public async Task<IEnumerable<CategoriaReadAdminDto>> ListarCategoriasAdmin()
        {
            var categorias = await _appDbContext.categorias
                .ToListAsync();

            if (categorias == null)
                return new List<CategoriaReadAdminDto>(); // devuelvo una lista vacia en vez de null porque es una buena practica 

            return _mapper.Map<IEnumerable<CategoriaReadAdminDto>>(categorias);
        }

        public async Task<IEnumerable<CategoriaReadDto>> ListarCategorias()
        {
            var categorias = await _appDbContext.categorias
                .Where(c => c.Activo == true)
                .ToListAsync();

            if (categorias == null)
                return new List<CategoriaReadDto>(); // devuelvo una lista vacia en vez de null porque es una buena practica 

            return _mapper.Map<IEnumerable<CategoriaReadDto>>(categorias);
        }

        public async Task<CategoriaReadAdminDto> ActualizarCategoria(int categoriaId, CategoriaUpdateDto categoriaUpdateDto)
        {
            var categoria = await ValidarExistenciaCategoria(categoriaId);

            _mapper.Map(categoriaUpdateDto, categoria);

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaReadAdminDto>(categoria);
        }

        public async Task EliminarCategoria(int idCategoria)
        {
           var categoria = await ValidarExistenciaCategoria(idCategoria);

            categoria.Activo = false;

            await DesactivarProductosCascada(categoria);

            await _appDbContext.SaveChangesAsync();

        }

        public async Task EditarEstadoCategoria(int categoriaId, bool nuevoEstado)
        {
            var categoria = await ValidarExistenciaCategoria(categoriaId);

            categoria.Activo = nuevoEstado;

            if(!nuevoEstado)
                await DesactivarProductosCascada(categoria);
            
            if(nuevoEstado)
                await ActivarProductosCascada(categoria);
            

            await _appDbContext.SaveChangesAsync();
        }


        //METODOS SECUNDARIOS
        private async Task<Categoria> ValidarExistenciaCategoria(int idCategoria)
        {
            var categoria = await _appDbContext.categorias
                .Include(c => c.Productos)
               .FirstOrDefaultAsync(c => c.Id == idCategoria);

            if (categoria == null)
                throw new KeyNotFoundException($"La Categoria con el ID {idCategoria} No Existe");

            return categoria;
        }

        private async Task DesactivarProductosCascada(Categoria categoria)
        {
            foreach (var producto in categoria.Productos)
            {
                producto.Activo = false;
            }
        }

        private async Task ActivarProductosCascada(Categoria categoria)
        {
            foreach (var producto in categoria.Productos)
            {
                producto.Activo = true;
            }
        }
    }
}
