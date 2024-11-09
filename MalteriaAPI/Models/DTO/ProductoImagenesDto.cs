namespace MalteriaAPI.Models.DTO
{
    public class ProductoImagenesDto
    {
        public int? Id { get; set; }
        public int ProductoId { get; set; }
        public string ImagenUrl { get; set; }
    }
}
