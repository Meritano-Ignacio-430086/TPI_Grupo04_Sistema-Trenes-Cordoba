namespace TPGI_Trenes.DTO
{
    public class Tickets_ResponseDTO
    {
        public int IdCliente { get; set; }
        public int IdTren { get; set; }
        public int IdRecorrido { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
        public List<DetalleTicketsDTO>? DetalleTickets { get; set; }
    }
}
