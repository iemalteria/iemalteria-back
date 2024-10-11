using System;
using System.Collections.Generic;

namespace MalteriaAPI.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
