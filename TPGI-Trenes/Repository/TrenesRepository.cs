using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;


namespace TPGI_Trenes.Repository
{
    public class TrenesRepository : ITrenesRepository
    {
        private readonly TrenesContext bd;
        public TrenesRepository(TrenesContext bd)
        {
            this.bd = bd;
        }
        public async Task<List<Trenes>> GetAllTrenes()
        {
            List<Trenes> trenes = await bd.Trenes.ToListAsync();
            if (trenes == null)
            {
                throw new ArgumentException("1"); //No existe el tren solicitada 
            }
            return trenes;
        }

        public async Task<List<Trenes>> GetByIdTren(int IdTren)
        {
            List<Trenes> trenes = await bd.Trenes.Include(a => a.IdTren).ToListAsync();
            if (trenes == null)
            {
                throw new ArgumentException("1"); //No existe el tren solicitada 
            }
            return trenes;
        }
    }
}
