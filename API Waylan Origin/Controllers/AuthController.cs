using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace API_Waylan_Origin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // POST api/<AuthController>
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioReadDto>> Registrar(UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                var resultado = await _authService.Registrar(usuarioCreateDto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar Usuario: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UsuarioLoginRequestDto LoginDto)
        {
            var token = await _authService.Login(LoginDto.Email, LoginDto.Password);

            if (token == null)
                return Unauthorized("Correo o Contraseña incorrecta");

            return Ok(new { token });
        }


    }
}
