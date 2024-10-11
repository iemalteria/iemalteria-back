using System;
using System.Collections.Generic;

namespace MalteriaAPI.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string? Titulo { get; set; }

    public string? Contenido { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public string? Estado { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
