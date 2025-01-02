namespace MalteriaAPI.Models.DTO
{
    public class Visitas
    {
        public int Id { get; set; }
        public string Pagina { get; set; }
        public DateTime FechaVisita { get; set; }
        public string IpUsuario { get; set; }
        public string Navegador { get; set; }
    }
}
