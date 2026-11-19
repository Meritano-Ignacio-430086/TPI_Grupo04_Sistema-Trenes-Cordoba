using TPGI_Trenes.DTO;

namespace TPGI_Trenes.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync (UsuarioLoginDTO request);
    }
}