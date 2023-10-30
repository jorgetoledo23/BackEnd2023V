using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/v1/controller")]
    [Authorize(Roles = "Administrador")]
    public class DepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context; //Field

        //Constructor
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


        [Route("getDptoById")]
        [HttpGet]
        public IActionResult getDptoById(int Id)
        {
            //Select * From TblDepartamentos Where Cod_Dpto = '1'
            return Ok(_context.TblDepartamentos.Where(x => x.CodigoDpto == Id).FirstOrDefault());
        }

        [Route("getDptoByName")]
        [HttpGet]
        public IActionResult getDptoByName(string Name)
        {
            //Select * From TblDepartamentos Where Cod_Dpto = '1'
            return Ok(_context.TblDepartamentos.Where(x => x.Nombre == Name).FirstOrDefault());
        }

        [Route("getDptoByFilter")]
        [HttpGet]
        public IActionResult getDptoByFilter(string Filter)
        {
            //Select * From TblDepartamentos Where Cod_Dpto = '1'
            return Ok(_context.TblDepartamentos.Where(x => x.Nombre.Contains(Filter)).ToList());
        }

        [HttpDelete]
        [Route("delDpto")]
        public async Task<IActionResult> delDpto(int id)
        {
            var dpto = await _context.TblDepartamentos
                .Where(d => d.CodigoDpto.Equals(id)).FirstOrDefaultAsync();

            if(dpto == null)
            {
                return NotFound();
            }
            _context.Remove(dpto);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut]
        [Route("uptDpto")]
        public async Task<IActionResult> uptDpto(Departamento dpto)
        {
            if (dpto == null) return NotFound();
            _context.Update(dpto);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
