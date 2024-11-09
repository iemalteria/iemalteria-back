using MalteriaAPI.Contexts;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public ProductosController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _dbContext.Productos.OrderBy(p => p.Categoria).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _dbContext.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            return Ok(producto);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] ProductosDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoProducto = new ProductosDto
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Precio = request.Precio,
                Precio2 = request.Precio2,
                Categoria = request.Categoria,
                Tipo = request.Tipo,
                Activo = request.Activo,
                VideoUrl = request.VideoUrl,

            };

            _dbContext.Productos.Add(nuevoProducto);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ProductosDto request)
        {
            var productoExistente = await _dbContext.Productos.FindAsync(id);

            if (productoExistente == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            productoExistente.Nombre = request.Nombre;
            productoExistente.Descripcion = request.Descripcion;
            productoExistente.Precio = request.Precio;
            if(request.Precio2 != 0 || request.Precio2 != null)
            productoExistente.Precio2 = (decimal)request.Precio2;

            productoExistente.Categoria = request.Categoria;
            productoExistente.Tipo = request.Tipo;
            productoExistente.Activo = request.Activo;
            productoExistente.VideoUrl = request.VideoUrl;

            await _dbContext.SaveChangesAsync();

            return Ok(productoExistente);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _dbContext.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            _dbContext.Productos.Remove(producto);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Producto eliminado exitosamente" });
        }
    }
}
