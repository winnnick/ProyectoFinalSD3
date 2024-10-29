using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using WebApplication1.Models;
namespace WebApplication1.Data
{
    public class Recursos : DbContext
    {
        public Recursos(DbContextOptions options) : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Vacaciones> vacaciones { get; set; }
        public DbSet<Capacitaciones> capacitaciones { get;set; }
        public DbSet<Informes> informes { get; set; }   
    }
}
