using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MalteriaAPI.Contexts;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitasController : ControllerBase
    {
        private readonly DBContext _context;

        public VisitasController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Visitas
        [HttpGet]
        public async Task<IActionResult> GetVisitas()
        {
            var visitas = _context.Visitas.OrderByDescending(v => v.FechaVisita).ToList();
            return StatusCode(StatusCodes.Status200OK, new { value = visitas });
        }

        // GET: api/Visitas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisita(int id)
        {
            var visita = await _context.Visitas.FindAsync(id);

            if (visita == null)
            {
                return NotFound();
            }

            return Ok(visita);
        }

        // POST: api/Visitas
        [HttpPost]
        public async Task<IActionResult> CreateVisita([FromBody] Visitas nuevaVisita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            nuevaVisita.FechaVisita = DateTime.Now; // Aseguramos que se registre con la fecha actual
            _context.Visitas.Add(nuevaVisita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVisita), new { id = nuevaVisita.Id }, nuevaVisita);
        }

        // DELETE: api/Visitas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisita(int id)
        {
            var visita = await _context.Visitas.FindAsync(id);

            if (visita == null)
            {
                return NotFound();
            }

            _context.Visitas.Remove(visita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Visitas/filter
        [HttpGet("filter")]
        public IActionResult FilterVisitas([FromQuery] string? pagina, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var visitasQuery = _context.Visitas.AsQueryable();

            if (!string.IsNullOrEmpty(pagina))
            {
                visitasQuery = visitasQuery.Where(v => v.Pagina.Contains(pagina));
            }

            if (fechaInicio.HasValue)
            {
                visitasQuery = visitasQuery.Where(v => v.FechaVisita >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                visitasQuery = visitasQuery.Where(v => v.FechaVisita <= fechaFin.Value);
            }

            var visitas = visitasQuery.OrderByDescending(v => v.FechaVisita).ToList();
            return Ok(visitas);
        }
    }
}
