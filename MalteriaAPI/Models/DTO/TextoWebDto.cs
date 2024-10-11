namespace MalteriaAPI.Models.DTO
{
    public class TextoWebDto
    {
        public int Id { get; set; } // Suponiendo que la tabla tiene una columna de ID
        public string Seccion { get; set; }
        public string Texto { get; set; }
        public string ImagenUrl { get; set; }
        public string AltText { get; set; }
        public string Titulo { get; set; }
    }
}
