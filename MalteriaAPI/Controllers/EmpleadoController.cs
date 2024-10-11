using MalteriaAPI.Contexts;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public EmpleadoController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _dbContext.Empleado.OrderBy(e => e.Sede).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var empleado = await _dbContext.Empleado.FindAsync(id);

            if (empleado == null)
            {
                return NotFound(new { message = "Empleado no encontrado" });
            }

            return Ok(empleado);
        }

        [HttpPost]
        [Authorize]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] EmpleadoDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoEmpleado = new EmpleadoDto
            {
                Nombre = request.Nombre,
                ImagenUrl = request.ImagenUrl,
                Descripcion = request.Descripcion,
                Sede = request.Sede,
                VideoUrl = request.VideoUrl // Nueva columna incluida
            };

            _dbContext.Empleado.Add(nuevoEmpleado);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoEmpleado.Id }, nuevoEmpleado);
        }

        [HttpPut]
        [Authorize]
        [Route("Actualizar/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] EmpleadoDto request)
        {
            var empleadoExistente = await _dbContext.Empleado.FindAsync(id);

            if (empleadoExistente == null)
            {
                return NotFound(new { message = "Empleado no encontrado" });
            }

            empleadoExistente.Nombre = request.Nombre;
            empleadoExistente.ImagenUrl = request.ImagenUrl;
            empleadoExistente.Descripcion = request.Descripcion;
            empleadoExistente.Sede = request.Sede;
            empleadoExistente.VideoUrl = request.VideoUrl; // Nueva columna incluida

            await _dbContext.SaveChangesAsync();

            return Ok(empleadoExistente);
        }

        [HttpDelete]
        [Authorize]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _dbContext.Empleado.FindAsync(id);

            if (empleado == null)
            {
                return NotFound(new { message = "Empleado no encontrado" });
            }

            _dbContext.Empleado.Remove(empleado);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Empleado eliminado exitosamente" });
        }
    }
}
