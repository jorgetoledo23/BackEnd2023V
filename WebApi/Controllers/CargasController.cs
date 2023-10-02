using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/v3/[controller]")] // https://miapi.cl/api/v3/cargas/getCargasTrab
    [ApiController]
    public class CargasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CargasController(AppDbContext context) // def __init__(self)
        {
            _context = context;
        }

        [HttpGet] // Leer datos
        [Route("getCargasTrab")]
        //Ir a buscar a la BD todas las cargas de X trabajador
        public async Task<IActionResult> getCargasTrab(string rut)
        {
            var cargas = await _context.TblCargas
                .Where(c => c.RutTrabajador == rut).ToListAsync();
            return Ok(cargas);
        }

        [HttpPost]
        [Route("addCarga")]
        public async Task<IActionResult> addCarga(CargaFamiliarDTO CargaDto)
        {
            try
            {
                var Carga = new CargaFamiliar()
                {
                    Rut = CargaDto.Rut,
                    Nombre = CargaDto.Nombre,
                    Apellido = CargaDto.Apellido,
                    Correo = CargaDto.Correo,
                    Telefono = CargaDto.Telefono,
                    Direccion = CargaDto.Direccion,
                    Comuna = CargaDto.Comuna,
                    Region = CargaDto.Region,
                    FechaNacimiento = CargaDto.FechaNacimiento,
                    RutTrabajador = CargaDto.RutTrabajador
                };

                _context.Add(Carga);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception E)
            {
                return BadRequest(E.Message);
            }
            
        }


    }
}
