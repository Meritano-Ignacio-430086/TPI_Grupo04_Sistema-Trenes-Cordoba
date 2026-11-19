using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Controller
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly IUserRepository repo;

        public AuthController(IAuthService service,IUserRepository repo)
        {
            this.service = service;
            this.repo = repo;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioLoginDTO usuario)
        {
            var token = await service.LoginAsync(usuario);
            if(token == null)
            {
                return Unauthorized(new {mensaje = "Credenciales invalidas"});
            }
            return Ok(new{token});
        }

        [HttpPost]
        [Route("registro")]
        public async Task<ActionResult> Registro([FromBody] UsuarioRegistroDTO usuarios)
        {
            try
            {
                if(usuarios == null || string.IsNullOrEmpty(usuarios.Email) || string.IsNullOrEmpty(usuarios.Password))
                {
                    return BadRequest("Faltan datos");
                }
                var usuarioExistente = await repo.ObtenerCorreoAsync(usuarios.Email);
                if(usuarioExistente != null)
                {
                    return BadRequest("Ya existe el usuario");
                }
                var newusuario = new Usuario
                {
                    Email = usuarios.Email
                };

                var hash = new PasswordHasher<Usuario>();
                newusuario.Clave = hash.HashPassword(newusuario, usuarios.Password);

                await repo.RegistroUsuarios(newusuario);
                return Ok("Usuario generado correcto");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new{message=$"Error interno: {ex.Message}"});
            }
        }
    }
}