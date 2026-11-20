using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class DetalleTicketsDTO
    {
        public int IdDetTicket { get; set; }

        public int IdTrenesVagones { get; set; }

        public int IdTrenRecorridoEst { get; set; }

        public int IdTicket { get; set; }

        public decimal? Precio { get; set; }
    }
}