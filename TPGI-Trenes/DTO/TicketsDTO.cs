using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class TicketsDTO
    {
        public int IdCliente { get; set; }
        public int IdTren { get; set; }
        public int IdRecorrido { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
        public List<DetalleTicketsDTO> DetalleTickets { get; set; }
    }
}