using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext db;

        public AuthController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginIn(LoginInViewModel lvm)
        {
            Usuario user;
            if(db.TblUsuarios.ToList().Count == 0)
            {
                user = new Usuario();
                user.Rol = "SuperAdministrador";
                user.Username = "SuperAdministrador";
                user.Correo = "admin@midominio.cl";
                user.Name = "SuperAdministrador";
                user.isActive = true;
                CreatePasswordHash("SuperAdministrador", out byte[] hash, out byte[] salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                db.Add(user);
                db.SaveChanges();
            };
            user = db.TblUsuarios.FirstOrDefault(x=> x.Correo == lvm.correo);
            if(user == null)
            {
                ModelState.AddModelError("", "Usuario NO Encontrado!");
                return View(lvm);
            }

            if(VerifyPasswordHash(lvm.password, user.PasswordSalt, user.PasswordHash))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Correo),
                    new Claim(ClaimTypes.Role, user.Rol),
                };

                var Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var Principal = new ClaimsPrincipal(Identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    Principal, 
                    new AuthenticationProperties { IsPersistent = true });
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Password Incorrecta!");
            return View(lvm);
        }

        public async Task<RedirectToActionResult> LogOut()
        {
            //TODO:Logout
            await HttpContext.SignOutAsync();
            return RedirectToAction("LoginIn", "Auth");
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


    }
}