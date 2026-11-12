using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
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


        public async Task<List<Tickets>> GetByClienteId(int id)
        {
            List<Tickets> tickets =  await bd.Tickets.Where(p=>p.IdCliente==id).ToListAsync();
            return tickets;
        }
    }
}
