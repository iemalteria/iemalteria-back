using MalteriaAPI.Contexts;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MalteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public BlogController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _dbContext.Blogs
                .OrderByDescending(b => b.FechaPublicacion)
                .ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }


        // Método para obtener un blog por ID
        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound(new { message = "Blog no encontrado" });
            }

            return Ok(blog);
        }

        // Método para crear un nuevo blog
        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] BlogDto request)
        {
            // Validación del modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Crear un nuevo objeto Blog
            var nuevoBlog = new Blog
            {
                Titulo = request.titulo,
                Contenido = request.contenido,
                FechaPublicacion = request.fechaPublicacion, // Asigna la fecha proporcionada en el DTO
                Estado = request.estado, // Asigna el estado proporcionado en el DTO
                IdUsuario = request.idUsuario, // Asigna el ID del usuario del DTO
                CategoriaId = request.CategoriaId
            };

            // Agregar el nuevo blog a la base de datos
            _dbContext.Blogs.Add(nuevoBlog);
            await _dbContext.SaveChangesAsync();

            // Devolver la respuesta con el nuevo blog creado
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoBlog.Id }, nuevoBlog);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("PorCategoria/{categoriaId}")]
        public async Task<IActionResult> ObtenerPorCategoriaId(int categoriaId)
        {
            var blogs = await _dbContext.Blogs
                .Where(b => b.CategoriaId == categoriaId)
                .OrderByDescending(b => b.FechaPublicacion)
                .ToListAsync();

            if (!blogs.Any())
            {
                return NotFound(new { message = "No se encontraron blogs para esta categoría" });
            }

            return StatusCode(StatusCodes.Status200OK, new { value = blogs });
        }


        // Método para editar un blog existente
        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] BlogDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogExistente = await _dbContext.Blogs.FindAsync(id);

            if (blogExistente == null)
            {
                return NotFound(new { message = "Blog no encontrado" });
            }

            // Actualizar los campos del blog existente con los valores del DTO
            blogExistente.Titulo = request.titulo;
            blogExistente.Contenido = request.contenido;
            blogExistente.FechaPublicacion = request.fechaPublicacion;
            blogExistente.Estado = request.estado;
            blogExistente.IdUsuario = request.idUsuario;
            blogExistente.CategoriaId = request.CategoriaId;

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Blog actualizado exitosamente", blog = blogExistente });
        }

        // Método para eliminar un blog
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound(new { message = "Blog no encontrado" });
            }

            // Eliminar el blog de la base de datos
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Blog eliminado exitosamente" });
        }

    }
}
