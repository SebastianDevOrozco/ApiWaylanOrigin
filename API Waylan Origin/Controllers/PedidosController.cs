using API_Waylan_Origin.DTOs.PedidosDto;
using API_Waylan_Origin.Enums;
using API_Waylan_Origin.Interfaces.Pedidos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Waylan_Origin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {

        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        //metodo para refactorizar el codigo y obtener el token
        private int ObtenerUsuarioIdDelToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            // un error claro en caso de que el token venga corrupto
            if (claim == null)
                throw new UnauthorizedAccessException("Token inválido o sin identificador.");
            return int.Parse(claim.Value);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoReadDto>> CrearPedido(PedidoCreateDto pedidoCreate)
        {
            //le asigno el claim a la variable usuarioId
            int usuarioId = ObtenerUsuarioIdDelToken();

            var nuevoPedido = await _pedidoService.CrearPedido(usuarioId, pedidoCreate);
            return Ok(nuevoPedido);
        }

        [HttpGet("Lista pedidos usuario")]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> ListarPedidos()
        {
            int usuarioId = ObtenerUsuarioIdDelToken();

            var listaPedidos = await _pedidoService.ListarPedidos(usuarioId);
            return Ok(listaPedidos);
        }

        [HttpGet("Lista pedidos Admin")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<PedidoReadAdminDto>>> ListaTodosLosPedidos()
        {
            var listaPedidos = await _pedidoService.ListarTodosLosPedidos();
            return Ok(listaPedidos);
        }

        [HttpGet("{codigo}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<PedidoReadAdminDto>> PedidoCodigo(string codigo)
        {
            var pedido = await _pedidoService.PedidoCodigo(codigo);
            return Ok(pedido);
        }

        [HttpPatch("{codigo}/cambiar-estado")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CambiarEstado(string codigo, EstadoPedido nuevoEstado)
        {
            await _pedidoService.CambiarEstadoPedido(codigo, nuevoEstado);
            return NoContent();
        }
      
    }
}
