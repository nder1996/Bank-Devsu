using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Enums;
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

       // public DbSet<Reporte> Reportes { get; set; }

        public DbSet<Persona> Personas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración TPT: cada entidad en su propia tabla
            modelBuilder.Entity<Persona>().ToTable("Personas");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");

            // Configuración explícita para el enum TipoMovimiento
            modelBuilder.Entity<Movimiento>()
                .Property(e => e.TipoMovimiento)
                .HasConversion<int>();

            // Configuración explícita para el enum ReporteFormato
           /* modelBuilder.Entity<Reporte>()
                .Property(e => e.Formato)
                .HasConversion<int>();*/
        }
    }
}
