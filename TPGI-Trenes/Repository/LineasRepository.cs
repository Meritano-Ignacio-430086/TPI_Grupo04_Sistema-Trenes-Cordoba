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

       /*  public Task<List<Recorridos>> GetById(int NroLinea)
        {
            List<Recorridos>recorridos = repo.GetProcedimientoLineas(NroLinea)
        } */
    }
}
