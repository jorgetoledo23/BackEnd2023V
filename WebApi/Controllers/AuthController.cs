using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using WebApi.Model;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string pass)
        {
            // User existe
            var user = await _context.TblUsuarios.Where(x => x.Username.Equals(username)).FirstOrDefaultAsync();
            if (user == null) return BadRequest("Usuario no encontrado");
            if(VerifyPasswordHash(pass, user.PasswordSalt, user.PasswordHash))
            {
                var Token = CreateToken(user);
                return Ok(Token);
            }
            return BadRequest("Credenciales Incorrectas!");

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
            usuario.Rol = udto.Rol;

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

        private string CreateToken(Usuario user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Rol)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:TokenKey").Value));

            var credential = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credential
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
