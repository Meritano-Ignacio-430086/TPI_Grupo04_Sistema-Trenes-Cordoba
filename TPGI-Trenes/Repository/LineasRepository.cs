using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Repository
{
    public class LineasRepository : ILineasRepository
    {
        private readonly TrenesContext bd;

        public LineasRepository(TrenesContext bd)
        {
            this.bd = bd;
        }

        public async Task<List<Recorridos>> GetById(int NroLinea)
        {
            List<Recorridos> recorridos = await bd.Recorridos.Include(a => a.TrenesRecorridosEstaciones)
                                                        .Include(a => a.EstacionesRecorridos)
                                                        .Where(a => a.NroLinea == NroLinea).ToListAsync();
            if(recorridos == null)
            {
                throw new ArgumentException("1"); //No existe la linea solicitada 
            }
            return recorridos;
        }
    }
}
