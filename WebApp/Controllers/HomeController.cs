using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;

        public HomeController(AppDbContext appDbContext)
        {
            db = appDbContext;
        }

        public IActionResult Index()
        {
            
            var users = db.TblUsuarios.ToList();
            var dptos = db.TblDepartamentos.ToList();

            var indexViewModel = new IndexViewModel()
            {
                ListaUsuarios = users,
                ListaDepartamento = dptos
            };

            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}