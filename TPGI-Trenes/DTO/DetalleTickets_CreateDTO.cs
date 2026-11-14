using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class DetalleTickets_CreateDTO
    {

        public int IdTrenesVagones { get; set; }

        public int IdTrenRecorridoEst { get; set; }

        public decimal? Precio { get; set; }
    }
}