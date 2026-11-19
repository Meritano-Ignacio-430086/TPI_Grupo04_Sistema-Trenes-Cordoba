using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Mantenimientos_ResponseDTO
    {
        public string? TipoMantenimiento { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Empleado { get; set; }
        public string? Unidad { get; set; }
    }
}
