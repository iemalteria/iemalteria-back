namespace MalteriaAPI.Models.DTO
{
    public class EmpleadoDto
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Descripcion { get; set; }
        public string Sede { get; set; } // Sede principal malteria, sede la colonia, sede b porvenir
    }
}
