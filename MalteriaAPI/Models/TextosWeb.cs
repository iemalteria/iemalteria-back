using System;
using System.Collections.Generic;

namespace MalteriaAPI.Models;

public partial class TextosWeb
{
    public int Id { get; set; }

    public string? Seccion { get; set; }

    public string? Texto { get; set; }

    public string? ImagenUrl { get; set; }

    public string? AltText { get; set; }

    public string? Titulo { get; set; }
}
