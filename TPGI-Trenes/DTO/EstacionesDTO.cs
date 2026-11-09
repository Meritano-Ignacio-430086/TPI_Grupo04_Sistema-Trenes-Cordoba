using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class EstacionesDTO
    {
        public int IdEstacion { get; set; }
        public string? Ubicacion { get; set; }

        public string? Nombre { get; set; }

        public int? Plataforma { get; set; }

    }
}