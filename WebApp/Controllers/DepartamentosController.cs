using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "SuperAdministrador, Asd, Jefe")]
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

        [Authorize(Roles = "SuperAdministrador")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdministrador")]
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

        [Authorize(Roles = "SuperAdministrador")]
        public async Task<IActionResult> Edit(string Cod)
        {
            var dpto = db.TblDepartamentos.Find(Convert.ToInt32(Cod));
            if (dpto == null) return NotFound();
            return View(dpto);
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpPost]
        public async Task<IActionResult> Edit(Departamento D)
        {
            db.Update(D);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "SuperAdministrador")]
        public async Task<IActionResult> Eliminar(int Cod)
        {
            var dpto = db.TblDepartamentos.Find(Cod);
            if (dpto == null) return NotFound();
            db.Remove(dpto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
