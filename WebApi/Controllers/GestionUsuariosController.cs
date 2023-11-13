using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class GestionUsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public GestionUsuariosController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        [Route("LoginIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginIn(string correo, string contra){
            var usuarios = await _context.TblUsuarios.ToListAsync();
            if(usuarios.Count == 0)
            {
                //En la BD NO hay Usuarios
                var user = new Usuario();
                user.Id = Guid.NewGuid();
                user.Username = "SuperAdministrador";
                user.Name = "SuperAdministrador";
                user.Correo = "sadmin@midominio.cl";
                CreatePasswordHash("123456", out byte[] hash, out byte[] salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.Rol = "SuperAdministrador";
                user.isActive = true;
                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

            if (existe == null) return BadRequest("Usuario NO Existe!");

            if (existe.isActive == false) return BadRequest("Usuario Bloqueado");

            if (VerifyPasswordHash(contra, existe.PasswordSalt, existe.PasswordHash))
            {
                var Token = CreateToken(existe);
                return Ok(Token);
            }
            return BadRequest("Contraseña Incorrecta");




        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> AddUser(string nombre, string username, string correo, string password, string rol)
        {
            if(rol.Equals("Administrador") || rol.Equals("Asistente") || rol.Equals("Vendedor"))
            {
                var NewUser = new Usuario();
                NewUser.Id = Guid.NewGuid();
                NewUser.Name = nombre.ToLower();
                NewUser.Username = username.ToLower();
                NewUser.Correo = correo.ToLower();
                NewUser.Rol = rol.ToLower();
                NewUser.isActive = true;
                CreatePasswordHash(password, out byte[] hash, out byte[] salt);
                NewUser.PasswordHash = hash;
                NewUser.PasswordSalt = salt;
                _context.Add(NewUser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Error de Rol!");

        }

        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> DeleteUser(string correo)
        {
            var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);
            if (existe == null) return BadRequest("Usuario No Encontrado!");
            if (existe.Rol == "SuperAdministrador") return BadRequest("No se puede eliminar un SuperAdministrador!");
            _context.Remove(existe);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("BlockUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> BlockUser(string correo)
        {
            var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);
            if (existe == null) return BadRequest("Usuario No Encontrado!");
            if (existe.Rol == "SuperAdministrador") return BadRequest("No se puede Bloquear un SuperAdministrador!");
            existe.isActive = false;
            _context.Update(existe);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("UnBlockUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UnBlockUser(string correo)
        {
            var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);
            if (existe == null) return BadRequest("Usuario No Encontrado!");
            existe.isActive = true;
            _context.Update(existe);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("ChangePass")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> ChangePass(string correo, string newpass)
        {
            //TODO: Si la persona que intenta cambiar password es un SuperAdministrador permitir el cambio incluso si 
            //al usuario que quiere cambiar la password es SuperAdministrador
            var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

            var Rol = User.FindFirstValue(ClaimTypes.Role); //Obtenemos el Rol del usuario logueado

            if (existe == null) return BadRequest("Usuario No Encontrado!");

            CreatePasswordHash(newpass, out byte[] hash, out byte[] salt);
            existe.PasswordHash = hash;
            existe.PasswordSalt = salt;

            if (Rol.Equals("SuperAdministrador"))
            {
                _context.Update(existe);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                if(existe.Rol == "SuperAdministrador") return BadRequest("No se puede cambiar password a un SuperAdministrador!");
                _context.Update(existe);
                await _context.SaveChangesAsync();
                return Ok();
            }
  
            
        }

        [HttpPut]
        [Route("ChangeUserData")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> ChangeUserData(string correo, string nombre, string username, string newcorreo, string password, string rol)
        {
            if (rol.Equals("Administrador") || rol.Equals("Asistente") || rol.Equals("Vendedor"))
            {
                var existe = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

                if (existe == null) return BadRequest("Usuario No Encontrado!");

                if (existe.Rol == "SuperAdministrador") return BadRequest("No se puede cambiar password a un SuperAdministrador!");


                CreatePasswordHash(password, out byte[] hash, out byte[] salt);
                existe.PasswordHash = hash;
                existe.PasswordSalt = salt;
                existe.Correo = newcorreo;
                existe.Name = nombre;
                existe.Username = username;
                existe.Rol = rol;
                _context.Update(existe);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Error de Rol");
            
            
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
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordhash);
            }
        }

        private string CreateToken(Usuario user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.Role, user.Rol)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:TokenKey").Value));

            var credential = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credential
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
