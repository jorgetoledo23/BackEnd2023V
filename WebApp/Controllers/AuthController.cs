using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
      
        public async Task<IActionResult> LoginIn()
        {
            return View();
        }

        public RedirectToActionResult Logout()
        {
            //TODO:Logout
            return RedirectToAction("LoginIn", "Auth");
        }

    }
}