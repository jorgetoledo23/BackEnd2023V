using System.ComponentModel.DataAnnotations;

namespace WebApi.Model
{
    public class Departamento
    {
        // Primary Key
        [Key]
        public int CodigoDpto { get; set; }
        public string Nombre { get; set; }

    }

}
