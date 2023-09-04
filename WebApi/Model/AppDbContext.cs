using Microsoft.EntityFrameworkCore;

namespace WebApi.Model
{





    /*
     
    Clase EF Core para configurar la conexion a la BD
    
    Debe Heredar  de la Class DbContext

     */
    public class AppDbContext : DbContext
    {
        public DbSet<Departamento> TblDepartamentos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* Cadena de Conexion hacia el servidor de BD */
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApiBackEnd2023V;Integrated Security=True;");
        }

    }
}
