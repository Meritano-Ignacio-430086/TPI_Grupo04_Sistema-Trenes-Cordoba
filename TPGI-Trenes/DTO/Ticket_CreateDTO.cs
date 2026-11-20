using System;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.DTO
{
    public class Tickets_CreateDTO
    {
        public int DniEmpleado { get; set; }
        public int DniCliente { get; set; }
        public List<DetalleTickets_CreateDTO> DetalleTickets { get; set; } = new();
        public List<string> FormasPago { get; set; } = new();
    }
}
