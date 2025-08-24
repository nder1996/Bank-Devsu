using BancoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Infrastructure.Data
{
    public class BancoDbContext : DbContext
    {
        public BancoDbContext(DbContextOptions<BancoDbContext> options): base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }

        public DbSet<Persona> Personas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración TPT: cada entidad en su propia tabla
            modelBuilder.Entity<Persona>().ToTable("Personas");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
        }
    }
}
