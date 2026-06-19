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
            //valido si la categoria existe
            var categoria = await _appDbContext.categorias
                .FirstOrDefaultAsync(c => c.Id == categoriaId);

            if (categoria == null)
                throw new KeyNotFoundException($"La Categoria con el ID {categoriaId} No Existe");

            _mapper.Map(categoriaUpdateDto, categoria);

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaReadAdminDto>(categoria);
        }

        public async Task<bool> EliminarCategoria(int idCategoria)
        {
            var categoria = await _appDbContext.categorias
                .FirstOrDefaultAsync(c => c.Id == idCategoria);

            if (categoria == null)
                throw new KeyNotFoundException($"La Categoria con el ID {idCategoria} No Existe");

            categoria.Activo = false;

            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
