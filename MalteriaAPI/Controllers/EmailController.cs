using MalteriaAPI.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("enviar-pdf")]
        public IActionResult EnviarCorreoConPdf([FromForm] string destinatario, [FromForm] string asunto, [FromForm] string mensaje, IFormFile archivoPdf, [FromServices] EmailService emailService)
        {
            if (archivoPdf == null || archivoPdf.Length == 0)
                return BadRequest("Debe proporcionar un archivo PDF.");

            // Guardar temporalmente el archivo
            var rutaArchivo = Path.Combine(Path.GetTempPath(), archivoPdf.FileName);

            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                archivoPdf.CopyTo(stream);
            }

            // Enviar el correo
            emailService.EnviarCorreoConAdjunto(destinatario, asunto, mensaje, rutaArchivo);

            return Ok(new { message = "Correo enviado con éxito." });
        }
    }
}
