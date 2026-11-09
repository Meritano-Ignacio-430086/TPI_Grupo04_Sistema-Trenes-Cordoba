using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface ITrenesRepository
    {
        Task<List<Trenes>> GetAllTrenes();
        Task<List<Trenes>> GetByIdTren(int IdTren);
    }
}
