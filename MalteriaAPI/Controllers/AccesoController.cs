using MalteriaAPI.Contexts;
using MalteriaAPI.Custom;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DBContext _dbContext;
        private readonly Utilidades _utilidades;
        public AccesoController(DBContext dbContext, Utilidades utilidades)
        {
            _dbContext = dbContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDto objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.nombre,
                Correo = objeto.correo,
                Clave = _utilidades.encriptarSHA256(objeto.clave)

            };

            await _dbContext.Usuarios.AddAsync(modeloUsuario);
            await _dbContext.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto objeto)
        {
            var usuarioEncontrado = await _dbContext.Usuarios
                .Where(u => u.Correo == objeto.correo && u.Clave == _utilidades.encriptarSHA256(objeto.clave)
                ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado), nombre=usuarioEncontrado.Nombre});
            }
        }

        [HttpGet]
        [Route("ValidarToken")]
        public IActionResult ValidarToken([FromQuery] string token)
        {
            bool respuesta = _utilidades.validarToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
