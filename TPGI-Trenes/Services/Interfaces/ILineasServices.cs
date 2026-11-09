using TPGI_Trenes.Negocio;
using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface ILineasServices
    {
<<<<<<< HEAD
        Task <List<RecorridosDTO>> GetById (int NroLinea);
        Task <List<RecorridosDTO>> GetAll ();
=======
        Task <List<LineasDTO>> GetById (int NroLinea);
>>>>>>> origin/TPI_Grupo04_Sistema-Trenes-Cordoba
    }

}