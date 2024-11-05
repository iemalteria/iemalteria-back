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
    public class CategoriaBlogController : ControllerBase
    {
        private readonly DBContext _context;

        public CategoriaBlogController(DBContext context)
        {
            _context = context;
        }

        // GET: api/CategoriaBlog
        [HttpGet]
        public async Task<IActionResult> GetCategoriasBlog()
        {
            var categorias = await _context.CategoriasBlog.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = categorias });
        }

        // GET: api/CategoriaBlog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaBlog>> GetCategoriaBlog(int id)
        {
            var categoria = await _context.CategoriasBlog.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // POST: api/CategoriaBlog
        [HttpPost]
        public async Task<ActionResult<CategoriaBlog>> PostCategoriaBlog(CategoriaBlog categoria)
        {
            _context.CategoriasBlog.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoriaBlog), new { id = categoria.Id }, categoria);
        }

        // PUT: api/CategoriaBlog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaBlog(int id, CategoriaBlog categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaBlogExists(id))
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

        // DELETE: api/CategoriaBlog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaBlog(int id)
        {
            var categoria = await _context.CategoriasBlog.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.CategoriasBlog.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaBlogExists(int id)
        {
            return _context.CategoriasBlog.Any(e => e.Id == id);
        }
    }
}
