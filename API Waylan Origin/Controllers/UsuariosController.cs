using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Waylan_Origin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {

        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService UsuarioService)
        {
            _usuarioService = UsuarioService;
        }

        //metodo para refactorizar el codigo y obtener el token
        private int ObtenerUsuarioIdDelToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim.Value);
        }

        [HttpGet("Perfil")]
        public async Task<ActionResult<UsuarioReadDto>> InfoUsuario()
        {
            //le asigno el claim a la variable usuarioId
            int usuarioId = ObtenerUsuarioIdDelToken();
            var infoUsuario = await _usuarioService.InfoUsuario(usuarioId);
            return Ok(infoUsuario);
        }

        [HttpGet("ListaUsuarios")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> ListaUsuarios()
        {
            var listaUsuarios = await _usuarioService.ListaUsuarios();
            return Ok(listaUsuarios);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="1")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var resultado = await _usuarioService.DeleteUsuario(id);
            return NoContent();
        }

        [HttpPatch("{id}/cambiar-estado")]
        [Authorize(Roles ="1")]
        public async Task<IActionResult> CambioEstado(int id,[FromBody] bool nuevoEstado)
        {
            await _usuarioService.EditarEstadoUsuario(id,nuevoEstado);
            return NoContent();
        }

    }
}
