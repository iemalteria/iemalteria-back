﻿namespace MalteriaAPI.Models.DTO
{
    public class ProductosDto
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal? Precio2 { get; set; }
        public string ImagenUrl { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public bool? Activo {  get; set; }
    }
}
