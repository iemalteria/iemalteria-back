using MalteriaAPI.Contexts;
using MalteriaAPI.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBContext _dbContext;
        private readonly Utilidades _utilidades;

        public UsuariosController(DBContext dbContext, Utilidades utilidades)
        {
            _dbContext = dbContext;
            _utilidades = utilidades;
        }
        [HttpGet]
        [Route("ObtenerUsuario")]
        public async Task<IActionResult> ObtenerUsuario([FromQuery] int id)
        {
            var usuario = await _dbContext.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new { u.Correo, u.Nombre })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { isSuccess = false, mensaje = "Usuario no encontrado" });
            }

            return Ok(new { isSuccess = true, usuario });
        }

        [Route("ObtenerInfoUsuario")]
        [HttpGet]
        public IActionResult ObtenerInfoUsuario()
        {
            // Obtener el token del encabezado de autorización
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                return Unauthorized(new { isSuccess = false, mensaje = "Token no proporcionado" });
            }

            // Decodificar el token
            var claimsPrincipal = _utilidades.decodificarToken(token);

            if (claimsPrincipal == null)
            {
                return Unauthorized(new { isSuccess = false, mensaje = "Token inválido" });
            }

            // Obtener los valores del usuario a partir de los reclamos
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            var userEmailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

            if (userIdClaim == null || userEmailClaim == null)
            {
                return BadRequest(new { isSuccess = false, mensaje = "Reclamos no encontrados en el token" });
            }

            var userId = int.Parse(userIdClaim.Value);
            var userEmail = userEmailClaim.Value;

            // Devolver la información del usuario
            var usuario = new
            {
                Id = userId,
                Email = userEmail
            };

            return Ok(new { isSuccess = true, usuario });
        }
    }
}
