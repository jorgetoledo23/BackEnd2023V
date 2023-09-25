using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajadorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TrabajadorController(AppDbContext context)
        {
            _context = context;
        }

        //TODO: Peticion para buscar un trabajador por su Rut
        //TODO: Peticion para buscar trabajadores por año de nacimiento

        [HttpPost] //Tipo Peticion HTTP => GET Lectura
        [Route("addtrabajador")]
        public async Task<IActionResult> addT(TrabajadorDTO T)
        {
            //Insert Into TblTrabajadores 
            Trabajador t = new Trabajador()
            {
                Rut = T.Rut,
                Nombre = T.Nombre,
                Apellido = T.Apellido,
                Comuna = T.Comuna,
                Correo = T.Correo,
                Telefono = T.Telefono,
                Region = T.Region,
                Direccion = T.Direccion,
                FechaNacimiento = T.FechaNacimiento,
                DptoTrabajador = T.DptoTrabajador
            };


            _context.Add(t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("getAllTrab")]
        public async Task<IActionResult> getAllTrabajadores()
        {
            var trabs = await _context.TblTrabajadores.ToListAsync();
            return Ok(trabs);
        }




    }
}
