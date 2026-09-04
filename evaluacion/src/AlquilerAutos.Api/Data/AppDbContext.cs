using AlquilerAutos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasIndex(c => c.DocumentoIdentidad).IsUnique();
            entity.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            entity.Property(c => c.DocumentoIdentidad).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasIndex(v => v.Placa).IsUnique();
            entity.Property(v => v.Placa).HasMaxLength(15).IsRequired();
            entity.Property(v => v.Marca).HasMaxLength(60).IsRequired();
            entity.Property(v => v.Modelo).HasMaxLength(60).IsRequired();
            entity.Property(v => v.TarifaDiaria).HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.Property(c => c.Total).HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.Cliente)
                .WithMany(cl => cl.Contratos)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Vehiculo)
                .WithMany(v => v.Contratos)
                .HasForeignKey(c => c.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.Property(p => p.Monto).HasColumnType("decimal(10,2)");

            entity.HasOne(p => p.Contrato)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
