using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    //Code First 
    public class Departamento
    {       
        // Primary Key
        [Key]
        public int CodigoDpto { get; set; }

        [StringLength(10)]
        public string Nombre { get; set; }

        //[NotMapped]
        //public string Descripcion { get; set; }
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


    /* 
    Agregar Campos considerados necesarios para ambas Class
    Y Generar claves Foraneas hacia la tabla Trabajador
     */

    public class CargaFamiliar
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

        //Fk

        [ForeignKey("Trabajador")]
        public string RutTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }



    }

    public class ContactoEmergencia 
    {
        [Key]
        public int ContactoEmergenciaId { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Region { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        //Fk

        [ForeignKey("Trabajador")]
        public string RutTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }


    }


}
