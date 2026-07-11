using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.Wompi;
using API_Waylan_Origin.Interfaces.Wompi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Waylan_Origin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPagosService _pagosService;

        public PagosController(AppDbContext context, IPagosService pagosService)
        {
            _context = context;
            _pagosService = pagosService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> RecibirNotificacionWompi( WompiWebHookDto notificacion)
        {
           
            await _pagosService.ProcesarNotificacionAsync(notificacion);

            // Siempre respondemos 200 OK rápido para que Wompi sepa que el servidor está vivo
            return Ok();
        }
    }
}
