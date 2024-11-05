using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MalteriaAPI.Contexts;
using MalteriaAPI.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [AllowAnonymous]
        [Route("ObtenerUsuario")]
        public async Task<IActionResult> ObtenerUsuario([FromQuery] int id)
        {
            var usuario = await _dbContext.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new { u.Id,u.Correo, u.Nombre, u.Rol })
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
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                return Unauthorized(new { isSuccess = false, mensaje = "Token no proporcionado" });
            }

            var claimsPrincipal = _utilidades.decodificarToken(token);

            if (claimsPrincipal == null)
            {
                return Unauthorized(new { isSuccess = false, mensaje = "Token inválido" });
            }

            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            var userEmailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

            if (userIdClaim == null || userEmailClaim == null)
            {
                return BadRequest(new { isSuccess = false, mensaje = "Reclamos no encontrados en el token" });
            }

            var userId = int.Parse(userIdClaim.Value);
            var userEmail = userEmailClaim.Value;

            var usuario = new
            {
                Id = userId,
                Email = userEmail
            };

            return Ok(new { isSuccess = true, usuario });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ObtenerTodosUsuarios")]
        public async Task<IActionResult> ObtenerTodosUsuarios()
        {
            var usuarios = await _dbContext.Usuarios
                .Select(u => new { u.Id, u.Correo, u.Nombre, u.Rol })
                .ToListAsync();

            return Ok(new { isSuccess = true, value = usuarios });
        }

        [HttpPut]
        [Route("EditarUsuario")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioEditDto usuarioEditDto)
        {
            // Buscar el usuario en la base de datos por su ID
            var usuario = await _dbContext.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { isSuccess = false, mensaje = "Usuario no encontrado" });
            }

            // Actualizar los campos del usuario con los valores recibidos
            usuario.Nombre = usuarioEditDto.Nombre;
            usuario.Correo = usuarioEditDto.Correo;
            usuario.Rol = usuarioEditDto.Rol;

            // Guardar los cambios en la base de datos
            _dbContext.Usuarios.Update(usuario);
            await _dbContext.SaveChangesAsync();

            return Ok(new { isSuccess = true, mensaje = "Usuario actualizado correctamente" });
        }
    }

    // DTO para recibir los datos del usuario a editar
    public class UsuarioEditDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}
