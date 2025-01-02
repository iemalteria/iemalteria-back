namespace MalteriaAPI.Models.DTO
{
    public class RespuestaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }  // FK hacia Usuario
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public int ComentarioId { get; set; }
    }
}
