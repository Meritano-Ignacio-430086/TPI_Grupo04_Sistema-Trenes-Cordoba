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

        // public async Task<int> GetAsientosLibres(int nroLinea, string estacion, TimeOnly hora, string tipoVagon)
        // {
        //     var linea = await bd.TrenesRecorridosEstaciones.Include(r => r.IdTrenesVagonesNavigation).FirstOrDefaultAsync(r => r.NroLinea == nroLinea
        //                                                                                                                     && r.IdEstacionNavigation.Nombre == estacion
        //                                                                                                                     && r.HoraPartida == hora);
        //     if(linea == null)
        //     {
        //         throw new ArgumentException("3");// Linea null
        //     }
        //     int idTren = linea.IdTrenesVagonesNavigation.IdTren;
        //     int? Asiento = await bd.TrenesxVagones.Where(tv => tv.IdTren == idTren
        //                                 && tv.IdVagonNavigation.IdTipoVagonNavigation.Descripcion == tipoVagon)
        //                                 .Select(tv => tv.IdVagonNavigation.CantidadAsientos - bd.DetalleTickets.Count(dt => dt.IdTrenesVagones == tv.IdTrenesVagones
        //                                                                                                                 && dt.IdTrenRecorridoEst == linea.IdTrenRecorridoEst)).FirstOrDefaultAsync();
        //     if(Asiento == null)
        //     {
        //         throw new ArgumentException("4");// Asientos Libres null
        //     }                                                                                                            

        //     return (int)Asiento;
        // }

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

        public async Task<List<Tickets>> GetByDniCliente(int? dniCliente)
        {
            return await bd.Tickets.Where(t => t.IdClienteNavigation.NroDoc == dniCliente.Value).ToListAsync();

        }
        public async Task<List<Tickets>> GetAllCliente()
        {
            return await bd.Tickets.ToListAsync();
        }

        // public async Task<bool> GetByDniEmpleado(int dniEmpleado)
        // {
        //     if (dniEmpleado == null)
        //     {
        //         return await bd.Empleados.AnyAsync();
        //     }
        //     else
        //     {
        //         return await bd.Empleados.Where(e => e.NroDoc == dniEmpleado).AnyAsync();
        //     }
        // }

        // public async Task<bool> GetByLineasFilter(int nroLinea, string estacion, TimeOnly hora)
        // {
        //     return await bd.TrenesRecorridosEstaciones.Where(r => r.NroLinea == nroLinea
        //                                                         && r.IdEstacionNavigation.Nombre == estacion
        //                                                         && r.HoraPartida == hora).AnyAsync();
        // }

        // public async Task<bool> GetDescFormaPago(string descripcion)
        // {
        //     return await bd.FormasPago.Where(f => f.Descripcion == descripcion).AnyAsync();
        // }
    }
}
