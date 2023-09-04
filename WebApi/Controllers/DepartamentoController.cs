using Microsoft.AspNetCore.Mvc;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/v1/controller")]
    public class DepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepartamentoController(AppDbContext context)
        {
            _context = context;
        }


        [Route("addDpto")]
        [HttpPost]
        public IActionResult addDpto(Departamento dpto)
        {
            _context.Add(dpto);
            _context.SaveChanges();
            return Ok();
        }

        [Route("getAllDptos")]
        [HttpGet]
        public IActionResult getAllDptos()
        {
            //Select * From TblDepartamentos
            return Ok(_context.TblDepartamentos.ToList());
        }
    }
}
