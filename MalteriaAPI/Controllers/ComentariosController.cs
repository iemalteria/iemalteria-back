using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MalteriaAPI.Contexts;
using MalteriaAPI.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly DBContext _context;

        public ComentariosController(DBContext context)
        {
            _context = context;
        }

        // CRUD para Comentarios

        // Obtener todos los comentarios
        [HttpGet("comentarios")]
        public async Task<ActionResult<IEnumerable<ComentarioDto>>> GetComentarios()
        {
            var lista = await _context.Comentario.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
            
        }

        // Obtener un comentario específico
        [HttpGet("comentarios/{id}")]
        public async Task<ActionResult<ComentarioDto>> GetComentario(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        // Crear un nuevo comentario
        [HttpPost("comentarios")]
        public async Task<ActionResult<ComentarioDto>> CreateComentario(ComentarioDto comentario)
        {
            _context.Comentario.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComentario), new { id = comentario.Id }, comentario);
        }

        // Actualizar un comentario
        [HttpPut("comentarios/{id}")]
        public async Task<IActionResult> UpdateComentario(int id, ComentarioDto comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Eliminar un comentario
        [HttpDelete("comentarios/{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentario.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // CRUD para Respuestas

        // Obtener todas las respuestas de un comentario
        [HttpGet("comentarios/{comentarioId}/respuestas")]
        public async Task<ActionResult<IEnumerable<RespuestaDto>>> GetRespuestas(int comentarioId)
        {
            
            var lista = await _context.Respuestas
                .Where(r => r.ComentarioId == comentarioId)
                .ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        // Crear una nueva respuesta para un comentario
        [HttpPost("comentarios/{comentarioId}/respuestas")]
        public async Task<ActionResult<RespuestaDto>> CreateRespuesta(int comentarioId, RespuestaDto respuesta)
        {
            respuesta.ComentarioId = comentarioId;

            _context.Respuestas.Add(respuesta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRespuestas), new { comentarioId = comentarioId }, respuesta);
        }

        // Actualizar una respuesta
        [HttpPut("respuestas/{id}")]
        public async Task<IActionResult> UpdateRespuesta(int id, RespuestaDto respuesta)
        {
            if (id != respuesta.Id)
            {
                return BadRequest();
            }

            _context.Entry(respuesta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RespuestaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Eliminar una respuesta
        [HttpDelete("respuestas/{id}")]
        public async Task<IActionResult> DeleteRespuesta(int id)
        {
            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }

            _context.Respuestas.Remove(respuesta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Métodos auxiliares
        private bool ComentarioExists(int id)
        {
            return _context.Comentario.Any(e => e.Id == id);
        }

        private bool RespuestaExists(int id)
        {
            return _context.Respuestas.Any(e => e.Id == id);
        }
    }
}
