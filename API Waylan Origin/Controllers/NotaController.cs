using API_Waylan_Origin.DTOs.NotasDto;
using API_Waylan_Origin.Interfaces.Notas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Waylan_Origin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotaController : ControllerBase
    {
       private readonly INotaService _NotaService;

        public NotaController(INotaService NotaService)
        {
            _NotaService = NotaService;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<NotaReadDto>> CrearNota(NotaCreateDto notaCreate)
        {
            var notaNueva = await _NotaService.CrearNota(notaCreate);
            return Ok(notaNueva);
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<NotaReadDto>>> ListarNotas()
        {
            var notas = await _NotaService.ListarNotas();
            return Ok(notas);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<NotaReadDto>> ActualizarNota(int id, NotaUpdateDto notaUpdate)
        {
            var notaActualizada = await _NotaService.ActualizarNota(id, notaUpdate);
            return Ok(notaActualizada);
        }
}}
