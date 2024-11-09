using MalteriaAPI.Contexts;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoImagenesController : ControllerBase
    {
        private readonly DBContext _context;

        public ProductoImagenesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ProductoImagenes

        [HttpGet]
        public async Task<IActionResult> GetProductoImagenes()
        {
            var lista = await _context.ProductoImagenes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        // GET: api/ProductoImagenes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoImagenesDto>> GetProductoImagenes(int id)
        {
            var productoImagenes = await _context.ProductoImagenes.FindAsync(id);

            if (productoImagenes == null)
            {
                return NotFound();
            }

            return new ProductoImagenesDto
            {
                Id = productoImagenes.Id,
                ProductoId = productoImagenes.ProductoId,
                ImagenUrl = productoImagenes.ImagenUrl
            };
        }

        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<ProductoImagenesDto>> GetProductoImagenesByProductoId(int productoId)
        {
            // Busca las imágenes asociadas al productoId proporcionado
            var productoImagenes = await _context.ProductoImagenes
                .Where(ci => ci.ProductoId == productoId)
                .ToListAsync();

            // Verifica si no se encontró ninguna imagen
            if (productoImagenes == null || !productoImagenes.Any())
            {
                return NotFound();
            }

            // Devuelve las imágenes como una lista de DTOs
            var result = productoImagenes.Select(ci => new ProductoImagenesDto
            {
                Id = ci.Id,
                ProductoId = ci.ProductoId,
                ImagenUrl = ci.ImagenUrl
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, new { value = result }); // Devuelve un 200 con la lista de imágenes
        }
        // POST: api/ProductoImagenes
        [HttpPost]
        public async Task<ActionResult<ProductoImagenesDto>> PostProductoImagenes(ProductoImagenesDto productoImagenesDto)
        {
            var productoImagenes = new ProductoImagenesDto
            {
                ProductoId = productoImagenesDto.ProductoId,
                ImagenUrl = productoImagenesDto.ImagenUrl
            };

            _context.ProductoImagenes.Add(productoImagenes);
            await _context.SaveChangesAsync();

            // Devuelve solo el DTO con las propiedades deseadas
            return CreatedAtAction(nameof(GetProductoImagenes), new { id = productoImagenes.Id }, new ProductoImagenesDto
            {
                Id = productoImagenes.Id,
                ProductoId = productoImagenes.ProductoId,
                ImagenUrl = productoImagenes.ImagenUrl
            });
        }

        // PUT: api/ProductoImagenes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductoImagen(int id, [FromBody] ProductoImagenesDto productoImagen)
        {


            _context.Entry(productoImagen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoImagenExists(id))
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

        // DELETE: api/ProductoImagenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductoImagen(int id)
        {
            var productoImagen = await _context.ProductoImagenes.FindAsync(id);
            if (productoImagen == null)
            {
                return NotFound();
            }

            _context.ProductoImagenes.Remove(productoImagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoImagenExists(int id)
        {
            return _context.ProductoImagenes.Any(e => e.Id == id);
        }
    }
}
