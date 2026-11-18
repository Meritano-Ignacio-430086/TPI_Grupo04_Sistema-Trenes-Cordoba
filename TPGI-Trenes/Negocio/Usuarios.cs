using System;
using System.Collections.Generic;

namespace TPGI_Trenes.Negocio;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Clave { get; set; } = string.Empty;
}