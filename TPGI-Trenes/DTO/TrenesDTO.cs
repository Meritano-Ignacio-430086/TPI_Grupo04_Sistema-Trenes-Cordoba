using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class TrenesDTO
    {
        public int IdTren { get; set; }

        public RecorridosDTO? NroUnidad { get; set; }

        public string? Modelo { get; set; }

        public int? Capacidad { get; set; }
    }
}