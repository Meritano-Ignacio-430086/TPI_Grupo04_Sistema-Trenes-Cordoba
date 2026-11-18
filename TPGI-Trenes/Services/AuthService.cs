using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;

namespace TPGI_Trenes.Services.Interfaces
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository repo; 
        private readonly JwtSettings jwt;
        private readonly PasswordHasher<Usuario> _pass = new();

        public AuthService(IUserRepository repo, JwtSettings jwt)
        {
            this.repo = repo;
            this.jwt = jwt;
        }

        public async Task<string?> LoginAsync(UsuarioLoginDTO request)
        {
            var usuario = await repo.ObtenerCorreoAsync(request.UserEmail);
            if(usuario == null)
            {
                throw new ArgumentException ("1"); // No existe el usuario
            }
            var resultado = _pass.VerifyHashedPassword(usuario,usuario.Clave, request.Password);
            if( resultado == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException ("2"); // Autenticacion erronea
            }
            return GenerarToken(usuario);
        }

        private string? GenerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwt.Key);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwt.ExpiresInMinutes > 0 ? jwt.ExpiresInMinutes:30),
                Issuer = jwt.Issuer,
                Audience = jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);
            return tokenHandler.WriteToken(token);
        }
    }
}