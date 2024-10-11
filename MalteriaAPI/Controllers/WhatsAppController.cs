using MalteriaAPI.Models.DTO;
using MalteriaAPI.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly WhatsAppService _whatsAppService;

        public WhatsAppController(WhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpPost("enviar")]
        public IActionResult EnviarMensaje([FromBody] MensajeWhatsAppDto mensajeDto)
        {
            _whatsAppService.EnviarMensajeWhatsApp(mensajeDto.To, mensajeDto.From, mensajeDto.Body);
            return Ok("Mensaje enviado correctamente.");
        }
    }
}
