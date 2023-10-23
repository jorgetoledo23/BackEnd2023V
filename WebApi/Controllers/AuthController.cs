using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using WebApi.Model;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDTO udto)
        {
            if (udto == null) return BadRequest();

            var usuario = new Usuario();
            usuario.Name = udto.Name;
            usuario.Correo = udto.Correo;
            usuario.Username = udto.Username;

            CreatePasswordHash(udto.Password, out byte[] hash, out byte[] salt);

            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }     
        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordhash)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordhash);
            }
        }

    }
}
