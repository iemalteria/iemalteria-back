namespace MalteriaAPI.Models.DTO
{
    public class CreseDto
    {
        public int? Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string VideoUrl { get; set; }

        // Relación de uno a muchos con CreseImagenes
        
    }
}
