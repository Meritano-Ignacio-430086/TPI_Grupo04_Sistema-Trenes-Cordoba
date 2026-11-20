using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;

namespace TPGI_Trenes.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<Usuario?> ObtenerCorreoAsync(string mail);
        Task RegistroUsuarios (Usuario usuario);
    }
}