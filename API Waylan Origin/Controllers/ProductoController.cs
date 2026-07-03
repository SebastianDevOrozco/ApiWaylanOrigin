using API_Waylan_Origin.DTOs.ProductoDto;
using API_Waylan_Origin.Enums;
using API_Waylan_Origin.Interfaces.Producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Waylan_Origin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<ProductoReadAdminDto>> CrearProducto([FromForm] ProductoCreateDto productoCreate)
        {
            var producto = await _productoService.CrearProducto(productoCreate);
            return Ok(producto);
        }

        [HttpGet("Lista de productos Admin")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<ProductoReadAdminDto>>> ListarProductosAdmin()
        {
            var productos = await _productoService.ListarProductosAdmin();
            return Ok(productos);
        }

        [HttpGet("Lista de productos")]
        public async Task<ActionResult<IEnumerable<ProductoReadDto>>> ListarProductos()
        {
            var productos = await _productoService.ListarProductos();
            return Ok(productos);
        }

        [HttpGet("Lista de productos por tueste")]
        public async Task<ActionResult<IEnumerable<ProductoReadDto>>> ListaProductosTueste(Tueste tueste)
        {
            var productos = await _productoService.ListarProductosTueste(tueste);
            return Ok(productos);
        }

        [HttpGet("Lista de productos por proceso")]
        public async Task<ActionResult<IEnumerable<ProductoReadDto>>> ListaProductosProceso(Proceso proceso)
        {
            var productos = await _productoService.ListarProductosProceso(proceso);
            return Ok(productos);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<ProductoReadAdminDto>> ActualizarProducto(int id, [FromForm] ProductoUpdateDto productoUpdate)
        {
            var producto = await _productoService.ActualizarProducto(id, productoUpdate);
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            await _productoService.EliminarProducto(id);
            return NoContent();
        }

        [HttpPatch("{id}/cambiar-estado")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CambioEstado(int id, bool nuevoEstado)
        {
            await _productoService.EditarEstadoProducto(id, nuevoEstado);
            return NoContent();
        }
    }
}
