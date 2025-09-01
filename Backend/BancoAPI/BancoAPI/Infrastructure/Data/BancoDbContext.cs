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

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración TPT (Table Per Type) - cada tipo tiene su propia tabla
            modelBuilder.Entity<Persona>()
                .ToTable("Personas");
                
            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            // Configurar la relación entre Cuenta y Cliente
            modelBuilder.Entity<Cuenta>()
                .HasOne(c => c.ClienteNavigation)
                .WithMany(cl => cl.Cuentas)
                .HasForeignKey(c => c.ClienteId);

            // Configuración explícita para el enum TipoMovimiento
            modelBuilder.Entity<Movimiento>()
                .Property(e => e.TipoMovimiento)
                .HasConversion<int>();

        }
    }
}
