﻿namespace MalteriaAPI.Models.DTO
{
    public class BlogDto
    {
        public string titulo { get; set; }
        public string contenido { get; set; }
        public int idUsuario { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public string estado { get; set; }
        public int CategoriaId { get; set; }  // Nueva columna para la relación de categoría

    }
}
