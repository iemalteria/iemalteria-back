using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MalteriaAPI.Contexts;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreseImagenesController : ControllerBase
    {
        private readonly DBContext _context;

        public CreseImagenesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/CreseImagenes
       
        [HttpGet]
        public async Task<IActionResult> GetCreseImagenes()
        {
            var lista = await _context.CreseImagenes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        // GET: api/CreseImagenes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreseImagenesDto>> GetCreseImagenes(int id)
        {
            var creseImagenes = await _context.CreseImagenes.FindAsync(id);

            if (creseImagenes == null)
            {
                return NotFound();
            }

            return new CreseImagenesDto
            {
                Id = creseImagenes.Id,
                CreseId = creseImagenes.CreseId,
                ImagenUrl = creseImagenes.ImagenUrl
            };
        }

        [HttpGet("crese/{creseId}")]
        public async Task<ActionResult<CreseImagenesDto>> GetCreseImagenesByCreseId(int creseId)
        {
            // Busca las imágenes asociadas al creseId proporcionado
            var creseImagenes = await _context.CreseImagenes
                .Where(ci => ci.CreseId == creseId)
                .ToListAsync();

            // Verifica si no se encontró ninguna imagen
            if (creseImagenes == null || !creseImagenes.Any())
            {
                return NotFound();
            }

            // Devuelve las imágenes como una lista de DTOs
            var result = creseImagenes.Select(ci => new CreseImagenesDto
            {
                Id = ci.Id,
                CreseId = ci.CreseId,
                ImagenUrl = ci.ImagenUrl
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, new { value = result }); // Devuelve un 200 con la lista de imágenes
        }
        // POST: api/CreseImagenes
        [HttpPost]
        public async Task<ActionResult<CreseImagenesDto>> PostCreseImagenes(CreseImagenesDto creseImagenesDto)
        {
            var creseImagenes = new CreseImagenesDto
            {
                CreseId = creseImagenesDto.CreseId,
                ImagenUrl = creseImagenesDto.ImagenUrl
            };

            _context.CreseImagenes.Add(creseImagenes);
            await _context.SaveChangesAsync();

            // Devuelve solo el DTO con las propiedades deseadas
            return CreatedAtAction(nameof(GetCreseImagenes), new { id = creseImagenes.Id }, new CreseImagenesDto
            {
                Id = creseImagenes.Id,
                CreseId = creseImagenes.CreseId,
                ImagenUrl = creseImagenes.ImagenUrl
            });
        }

        // PUT: api/CreseImagenes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCreseImagen(int id, [FromBody]CreseImagenesDto creseImagen)
        {
            

            _context.Entry(creseImagen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreseImagenExists(id))
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

        // DELETE: api/CreseImagenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreseImagen(int id)
        {
            var creseImagen = await _context.CreseImagenes.FindAsync(id);
            if (creseImagen == null)
            {
                return NotFound();
            }

            _context.CreseImagenes.Remove(creseImagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CreseImagenExists(int id)
        {
            return _context.CreseImagenes.Any(e => e.Id == id);
        }
    }
}
