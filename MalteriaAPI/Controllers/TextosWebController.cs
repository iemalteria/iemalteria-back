using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MalteriaAPI.Contexts;
using MalteriaAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TextosWebController : ControllerBase
    {
        private readonly DBContext _context;

        public TextosWebController(DBContext context)
        {
            _context = context;
        }

        // GET: api/TextosWeb

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TextosWeb>>> GetTextosWeb()
        {
            return await _context.TextosWeb.ToListAsync();
        }

        // GET: api/TextosWeb/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TextosWeb>> GetTextosWeb(int id)
        {
            var textoWeb = await _context.TextosWeb.FindAsync(id);

            if (textoWeb == null)
            {
                return NotFound();
            }

            return textoWeb;
        }

        // PUT: api/TextosWeb/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTextosWeb(int id, [FromBody] TextosWeb textoWeb)
        {
            if (id != textoWeb.Id)
            {
                return BadRequest();
            }

            _context.Entry(textoWeb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TextosWebExists(id))
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

        private bool TextosWebExists(int id)
        {
            return _context.TextosWeb.Any(e => e.Id == id);
        }
    }
}
