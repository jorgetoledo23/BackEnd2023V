using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{

    /*
     
    Clase EF Core para configurar la conexion a la BD
    
    Debe Heredar  de la Class DbContext

     */
    public class AppDbContext : DbContext
    {
        public DbSet<Departamento> TblDepartamentos { get; set; }
        public DbSet<Trabajador> TblTrabajadores { get; set; }
        public DbSet<ContactoEmergencia> TblContactos { get; set; }
        public DbSet<CargaFamiliar> TblCargas { get; set; }
        public DbSet<Usuario> TblUsuarios { get; set; }

        /*
        public DbSet<CargaFamiliar> TblCargas { get; set; }
        public DbSet<ContactoEmergencia> TblContactos { get; set; }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* Cadena de Conexion hacia el servidor de BD */
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApiBackEnd2023V;Integrated Security=True;");
        }

    }


public class Usuario
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Correo { get; set; }
    public string Username { get; set; }
    public string Rol { get; set; }
    public bool isActive { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }


}
}
