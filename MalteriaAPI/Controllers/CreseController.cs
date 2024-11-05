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
    public class CreseController : ControllerBase
    {
        private readonly DBContext _context;

        public CreseController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Crese
        
        
        [HttpGet]
        public async Task<IActionResult> GetCrese()
        {
            var lista = await _context.Crese.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        // GET: api/Crese/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreseDto>> GetCrese(int id)
        {
            // Incluye CreseImagenes solo en el método GET por ID
            var crese = await _context.Crese
                .FirstOrDefaultAsync(c => c.Id == id);

            if (crese == null)
            {
                return NotFound();
            }

            return crese;
        }

        // POST: api/Crese
        [HttpPost]
        public async Task<ActionResult<CreseDto>> PostCrese(CreseDto crese)
        {
            // No incluye CreseImagenes en el método POST
            _context.Crese.Add(crese);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCrese),  crese);
        }

        // PUT: api/Crese/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrese(int id, CreseDto crese)
        {
           
            // No incluye CreseImagenes en el método PUT
            _context.Entry(crese).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreseExists(id))
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

        // DELETE: api/Crese/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrese(int id)
        {
            var crese = await _context.Crese.FindAsync(id);
            if (crese == null)
            {
                return NotFound();
            }

            // No incluye CreseImagenes en el método DELETE
            _context.Crese.Remove(crese);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CreseExists(int id)
        {
            return _context.Crese.Any(e => e.Id == id);
        }
    }
}
