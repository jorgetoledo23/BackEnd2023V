using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    //Code First 
    public class Departamento
    {
        // Primary Key
        [Key]
        public int CodigoDpto { get; set; }
        public string Nombre { get; set; }

    }

    public class Trabajador
    {
        [Key]
        
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


        //Clave Foranea

        [ForeignKey("Departamento")]
        public int DptoTrabajador { get; set; }
        public Departamento Departamento { get; set; }

    }



}
