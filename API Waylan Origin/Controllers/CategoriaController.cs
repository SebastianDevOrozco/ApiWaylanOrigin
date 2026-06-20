using API_Waylan_Origin.DTOs.CategoriaDto;
using API_Waylan_Origin.Interfaces.Categoria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Waylan_Origin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<CategoriaReadAdminDto>> CrearCategoria(CategoriaCreateDto createDto)
        {
            var categoria = await _categoriaService.CrearCategoria(createDto);
            return Ok(categoria);
        }

        [HttpGet("Lista Categorias Admin")]
        [Authorize(Roles ="1")]
        public async Task<ActionResult<IEnumerable<CategoriaReadAdminDto>>> ListaCategoriasAdmin()
        {
            var categorias = await _categoriaService.ListarCategoriasAdmin();
            return Ok(categorias);
        }

        [HttpGet("Lista Categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaReadDto>>> ListaCategorias()
        {
            var categorias = await _categoriaService.ListarCategorias();
            return Ok(categorias);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<CategoriaReadAdminDto>> ActualizarCategoria(int id, CategoriaUpdateDto updateDto)
        {
            var categoriaActualizada = await _categoriaService.ActualizarCategoria(id, updateDto);
            return Ok(categoriaActualizada);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            await _categoriaService.EliminarCategoria(id);
            return NoContent();
        }

        [HttpPatch("{id}/cambiar-estado")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CambioEstado(int id, [FromBody] bool nuevoEstado)
        {
            await _categoriaService.EditarEstadoCategoria(id, nuevoEstado);
            return NoContent();
        }
    }
    
}

