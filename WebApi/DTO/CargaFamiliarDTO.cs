using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class CargaFamiliarDTO
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Region { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public string RutTrabajador { get; set; }
    }
}
