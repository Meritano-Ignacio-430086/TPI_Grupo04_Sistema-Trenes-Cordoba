using System;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Tickets_CreateDTO
    {
        public int IdEmpleado { get; set; } //El que hace el ticket
        public int IdCliente { get; set; } // A quien se lo hace
        public DateTime FechaCompra { get; set; } //Cuando se lleva a cabo la compra del ticket
        public List<DetalleTickets_CreateDTO>? DetalleTickets { get; set; }
    }
}
