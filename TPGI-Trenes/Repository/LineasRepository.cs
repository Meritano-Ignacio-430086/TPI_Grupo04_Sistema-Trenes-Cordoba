using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Repository;

namespace TPGI_Trenes.Repository
{
    public class LineasRepository : ILineasRepository
    {
        private readonly TrenesContext bd;
        private readonly QueryRepository repo;

        public LineasRepository(TrenesContext bd,QueryRepository repo)
        {
            this.bd = bd;
            this.repo = repo;
        }

        public async Task<List<Recorridos>> GetAllLineas()
        {
            List<Recorridos> recorridos =  await bd.Recorridos.ToListAsync();
            if (recorridos == null)
            {
                throw new ArgumentException("1"); //No existe la linea solicitada 
            }
            return recorridos;
        }

        public async Task<List<Recorridos>> GetById(int NroLinea)
        {
            List<Recorridos>recorridos = await bd.Recorridos.Where(o=>o.NroLinea==NroLinea).ToListAsync();
            return recorridos;
        } 
    }
}
