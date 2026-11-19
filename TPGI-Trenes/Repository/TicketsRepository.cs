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
            var empleado = await bd.Empleados.FirstOrDefaultAsync(e=>e.NroDoc == ticket.DniEmpleado)/*Where(e => e.NroDoc == ticket.DniEmpleado).ToListAsync()*/;
            var cliente = await bd.Clientes.FirstOrDefaultAsync(c=>c.NroDoc == ticket.DniCliente)/*Where(c => c.NroDoc == ticket.DniCliente).ToListAsync()*/;
            var formas = new List<TicketsXFormapago>();
            foreach (var desc in ticket.FormasPago)
            {
                var formaPago = await bd.FormasPago.FirstOrDefaultAsync(f => f.Descripcion == desc);
                formas.Add(new TicketsXFormapago { IdFormaPago = formaPago.IdFormaPago });
            }
            var detalles = new List<DetalleTickets>();
            foreach (var dt in ticket.DetalleTickets)
            {
                //var hora = DateTime.Now; /*Fecha de compra fallida*/
                TimeSpan.TryParse(dt.HoraPartida, out var hora);
                var linea = await bd.TrenesRecorridosEstaciones.Include(r=>r.IdTrenesVagonesNavigation).FirstOrDefaultAsync(r=>r.NroLinea == dt.NroLinea
                                                                                                                            && r.IdEstacionNavigation.Nombre == dt.Estacion
                                                                                                                            && r.HoraPartida == TimeOnly.FromTimeSpan(hora));
                int idTren = linea.IdTrenesVagonesNavigation.IdTren;
                var trenesVagon = await bd.TrenesxVagones.Include(tv => tv.IdVagonNavigation).ThenInclude(v => v.IdServicioNavigation).Where(tv => tv.IdTren == idTren
                                                                                                                                                && tv.IdVagonNavigation.IdTipoVagonNavigation.Descripcion == dt.TipoVagon).Select(tv => new
                                                                                                                                                {
                                                                                                                                                    Tv = tv,
                                                                                                                                                    Vendidos = bd.DetalleTickets.Count(dt => dt.IdTrenesVagones == tv.IdTrenesVagones
                                                                                                                                                                                          && dt.IdTrenRecorridoEst == linea.IdTrenRecorridoEst)
                                                                                                                                                }).ToListAsync();
                var elegido = trenesVagon.FirstOrDefault(c=>c.Vendidos + detalles.Count(x=>x.IdTrenesVagones == c.Tv.IdTrenesVagones
                                                                                        && x.IdTrenRecorridoEst == linea.IdTrenRecorridoEst) < c.Tv.IdVagonNavigation.CantidadAsientos);
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

            //Tickets ticketaux = new Tickets
            //{
            //    IdEmpleado = ticket.IdEmpleado,
            //    IdCliente = ticket.IdCliente,
            //    Fecha = ticket.FechaCompra,
            //    DetalleTickets = ticket.DetalleTickets?.Select //Detalle tickets podría ser null
            //    (
            //        dt=>new DetalleTickets
            //        { 
            //            IdTrenesVagones = dt.IdTrenesVagones,
            //            IdTrenRecorridoEst = dt.IdTrenRecorridoEst,
            //            //Precio = dt.Precio Calcularlo con servicio
            //        }
            //        ).ToList()
            //};
            //bd.Tickets.Add(ticketaux);
            //await bd.SaveChangesAsync();
            //return true;
        }

        public async Task<List<Tickets>> GetByClienteId(int id)
        {
            List<Tickets> tickets =  await bd.Tickets.Where(p=>p.IdCliente==id).ToListAsync();
            return tickets;
        }

        public async Task<Clientes> GetCliente(int dniCliente)
        {
            throw new NotImplementedException();
            //return await bd.Clientes.Where(c => c.NroDoc == dniCliente).Select(c => (int?)c.IdCliente).FirstOrDefaultAsync();
        }

        public async Task<Empleados> GetEmpleado(int dniEmpleado)
        {
            throw new NotImplementedException();
            //return await bd.Empleados.Where(e => e.NroDoc == dniEmpleado).Select(e => (int)e.IdEmpleado).FirstOrDefaultAsync();

        }

        public async Task<FormasPago> GetFormaPago(string descripcion)
        {
            throw new NotImplementedException();
        }
    }
}
