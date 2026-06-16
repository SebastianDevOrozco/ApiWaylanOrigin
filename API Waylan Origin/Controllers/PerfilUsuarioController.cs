using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Waylan_Origin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilUsuarioController : ControllerBase 
    {

        private readonly IPerfilUsuarioService _perfilUsuarioService;

        public PerfilUsuarioController(IPerfilUsuarioService perfilUsuarioService)
        {
            _perfilUsuarioService = perfilUsuarioService;
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

            var infoUsuario = await _perfilUsuarioService.InfoUsuario(usuarioId);

            if (infoUsuario == null)
                NotFound("El usuario ya no se encuentra registrado.");

            return Ok(infoUsuario);
        }

    }
}
