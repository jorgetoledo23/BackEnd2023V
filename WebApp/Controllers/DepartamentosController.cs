using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly AppDbContext db;
        public DepartamentosController(AppDbContext appDbContext)
        {
            db = appDbContext;
        }
        public IActionResult Index()
        {
            return View(db.TblDepartamentos.ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departamento dpto)
        {
            if (ModelState.IsValid)
            {
                db.Add(dpto);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dpto);
            
        }

        public async Task<IActionResult> Edit(string Cod)
        {
            var dpto = db.TblDepartamentos.Find(Convert.ToInt32(Cod));
            if (dpto == null) return NotFound();
            return View(dpto);
        }
    }
}
