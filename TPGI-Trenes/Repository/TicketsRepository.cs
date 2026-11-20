using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Repository
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly TrenesContext bd;

        public TicketsRepository(TrenesContext bd)
        {
            this.bd = bd;
        }

        public async Task<bool> AddTicket(Tickets_CreateDTO ticket)
        {
            var empleado = await bd.Empleados.FirstOrDefaultAsync(e => e.NroDoc == ticket.DniEmpleado)/*Where(e => e.NroDoc == ticket.DniEmpleado).ToListAsync()*/;
            if (empleado == null)
            {
                throw new ArgumentException("4"); //No existe el empleado
            }
            var cliente = await bd.Clientes.FirstOrDefaultAsync(c => c.NroDoc == ticket.DniCliente)/*Where(c => c.NroDoc == ticket.DniCliente).ToListAsync()*/;
            if (cliente == null)
            {
                throw new ArgumentException("5"); // No existe el cliente
            }
            var formas = new List<TicketsXFormapago>();
            foreach (var desc in ticket.FormasPago)
            {
                var formaPago = await bd.FormasPago.FirstOrDefaultAsync(f => f.Descripcion == desc);
                if (formaPago == null)
                {
                    throw new ArgumentException("1"); //Forma de pago null
                }
                formas.Add(new TicketsXFormapago { IdFormaPago = formaPago.IdFormaPago });
            }
            var detalles = new List<DetalleTickets>();
            foreach (var dt in ticket.DetalleTickets)
            {
                //var hora = DateTime.Now; /*Fecha de compra fallida*/
                TimeSpan.TryParse(dt.HoraPartida, out var hora);
                var linea = await bd.TrenesRecorridosEstaciones.Include(r => r.IdTrenesVagonesNavigation).FirstOrDefaultAsync(r => r.NroLinea == dt.NroLinea
                                                                                                                            && r.IdEstacionNavigation.Nombre == dt.Estacion);
                // && r.HoraPartida >= TimeOnly.FromTimeSpan(hora));
                if (linea == null)
                {
                    throw new ArgumentException("5"); // No existe la linea
                }
                int idTren = linea.IdTrenesVagonesNavigation.IdTren;
                var trenesVagon = await bd.TrenesxVagones.Include(tv => tv.IdVagonNavigation).ThenInclude(v => v.IdServicioNavigation).Where(tv => tv.IdTren == idTren
                                                                                                                                                && tv.IdVagonNavigation.IdTipoVagonNavigation.Descripcion == dt.TipoVagon).Select(tv => new
                                                                                                                                                {
                                                                                                                                                    Tv = tv,
                                                                                                                                                    Vendidos = bd.DetalleTickets.Count(dt => dt.IdTrenesVagones == tv.IdTrenesVagones
                                                                                                                                                                                          && dt.IdTrenRecorridoEst == linea.IdTrenRecorridoEst)
                                                                                                                                                }).ToListAsync();
                var elegido = trenesVagon.FirstOrDefault(c => c.Vendidos + detalles.Count(x => x.IdTrenesVagones == c.Tv.IdTrenesVagones
                                                                                        && x.IdTrenRecorridoEst == linea.IdTrenRecorridoEst) < c.Tv.IdVagonNavigation.CantidadAsientos);
                if (elegido == null)
                {
                    throw new ArgumentException("2"); //Tren elegido null
                }
                detalles.Add(new DetalleTickets
                {
                    IdTrenRecorridoEst = linea.IdTrenRecorridoEst,
                    IdTrenesVagones = elegido.Tv.IdTrenesVagones
                });
            }

            bd.Tickets.Add(new Tickets
            {
                IdEmpleado = empleado.IdEmpleado,
                IdCliente = cliente.IdCliente,
                Fecha = DateTime.Now,
                DetalleTickets = detalles,
                TicketsXFormapago = formas
            });
            await bd.SaveChangesAsync();
            return true;
        }

        public async Task<List<Tickets>> GetByClienteId(int dni)
        {
            return await bd.Tickets.Include(t => t.IdClienteNavigation)
                                .Include(t => t.DetalleTickets)
                                    .ThenInclude(dt => dt.IdTrenRecorridoEstNavigation)
                                        .ThenInclude(te => te.IdEstacionNavigation)
                                .Include(t => t.DetalleTickets)
                                    .ThenInclude(dt => dt.IdTrenesVagonesNavigation)
                                        .ThenInclude(tv => tv.IdVagonNavigation)
                                            .ThenInclude(v => v.IdTipoVagonNavigation).ToListAsync();
        }

        public async Task<List<HistorialCompraDTO>> GetByDniCliente(int? dniCliente)
        {
            {
                if(dniCliente == null){throw new ArgumentException("1");}
            var filas = await bd.Tickets.Where(t => t.IdClienteNavigation.NroDoc == dniCliente.Value)
            .SelectMany(t => t.DetalleTickets.DefaultIfEmpty(), (t, dt) => new
            {
            IdCliente   = t.IdCliente,
            NroDoc      = dniCliente,
            TipoDoc     = t.IdClienteNavigation.IdTipoDocNavigation.Descripcion,
            IdTicket    = t.IdTicket,
            Fecha       = t.Fecha,
            IdDetTicket = (int?)dt.IdDetTicket,
            NroLinea    = (int?)dt.IdTrenRecorridoEstNavigation.NroLinea,
            Recorrido   = dt.IdTrenRecorridoEstNavigation.NroLineaNavigation.Descripcion,
            Estacion    = dt.IdTrenRecorridoEstNavigation.IdEstacionNavigation.Nombre,
            Tren        = dt.IdTrenesVagonesNavigation.IdTrenNavigation.NroUnidad,
            HoraPartida = dt.IdTrenRecorridoEstNavigation.HoraPartida,
            HoraLlegada = dt.IdTrenRecorridoEstNavigation.HoraLlegada,
            Precio      = dt.Precio
        })
        .OrderByDescending(x => x.Fecha)
        .ToListAsync();

        return filas.Select(f => new HistorialCompraDTO
        {
        Id_Cliente     = f.IdCliente ?? 0,
        NroDoc         = (int)dniCliente,
        Tipo_Documento = f.TipoDoc,
        Id_Ticket      = f.IdTicket,
        Fecha_Compra   = f.Fecha ?? default,
        Id_Det_Ticket  = f.IdDetTicket,
        Nro_Linea      = f.NroLinea,
        Recorrido      = f.Recorrido,
        Estacion       = f.Estacion,
        Tren           = f.Tren,
        Hora_Partida   = f.HoraPartida?.ToTimeSpan() ?? TimeSpan.Zero,
        Hora_Llegada   = f.HoraLlegada?.ToTimeSpan() ?? TimeSpan.Zero,
        Precio         = f.Precio ?? 0
        }).ToList();
        }       

        }
        public async Task<List<HistorialCompraDTO>> GetAllCliente()
        {
            {
            var filas = await bd.Tickets
            .SelectMany(t => t.DetalleTickets.DefaultIfEmpty(), (t, dt) => new
            {
            IdCliente   = t.IdCliente,
            NroDoc      = t.IdClienteNavigation.NroDoc,
            TipoDoc     = t.IdClienteNavigation.IdTipoDocNavigation.Descripcion,
            IdTicket    = t.IdTicket,
            Fecha       = t.Fecha,
            IdDetTicket = (int?)dt.IdDetTicket,
            NroLinea    = (int?)dt.IdTrenRecorridoEstNavigation.NroLinea,
            Recorrido   = dt.IdTrenRecorridoEstNavigation.NroLineaNavigation.Descripcion,
            Estacion    = dt.IdTrenRecorridoEstNavigation.IdEstacionNavigation.Nombre,
            Tren        = dt.IdTrenesVagonesNavigation.IdTrenNavigation.NroUnidad,
            HoraPartida = dt.IdTrenRecorridoEstNavigation.HoraPartida,
            HoraLlegada = dt.IdTrenRecorridoEstNavigation.HoraLlegada,
            Precio      = dt.Precio
        })
        .OrderByDescending(x => x.Fecha)
        .ToListAsync();

        return filas.Select(f => new HistorialCompraDTO
        {
        Id_Cliente     = f.IdCliente ?? 0,
        NroDoc         = f.NroDoc ?? 0,
        Tipo_Documento = f.TipoDoc,
        Id_Ticket      = f.IdTicket,
        Fecha_Compra   = f.Fecha ?? default,
        Id_Det_Ticket  = f.IdDetTicket,
        Nro_Linea      = f.NroLinea,
        Recorrido      = f.Recorrido,
        Estacion       = f.Estacion,
        Tren           = f.Tren,
        Hora_Partida   = f.HoraPartida?.ToTimeSpan() ?? TimeSpan.Zero,
        Hora_Llegada   = f.HoraLlegada?.ToTimeSpan() ?? TimeSpan.Zero,
        Precio         = f.Precio ?? 0
        }).ToList();
        }       
        }

        
    }
}
