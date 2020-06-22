 
using ArqNetCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArqNetCore.Configuration
{
    public class ArqNetCoreDbContext : DbContext
    {
        public ArqNetCoreDbContext(DbContextOptions<ArqNetCoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplyAttribute>()
                .HasKey(supplyAttribute => new { 
                    supplyAttribute.SupplyId, 
                    supplyAttribute.AttributeName
                });
            modelBuilder.Entity<SupplyTypeAttribute>()
                .HasKey(supplyTypeAttribute => new { 
                    supplyTypeAttribute.SupplyTypeId, 
                    supplyTypeAttribute.AttributeName
                });   
            modelBuilder.Entity<SupplyType>().HasData(
                new SupplyType
                {
                    Id = "MASCARA_PROTECTORA",
                    Description = "Máscara protectora"
                },
                new SupplyType
                {
                    Id = "BARBIJO",
                    Description = "Barbijo"
                },
                new SupplyType
                {
                    Id = "RESPIRADOR",
                    Description = "Respirador"
                },
                new SupplyType
                {
                    Id = "GUANTE",
                    Description = "Guante"
                },
                new SupplyType
                {
                    Id = "MEDICAMENTO",
                    Description = "Medicamento"
                }
            );
            modelBuilder.Entity<SupplyTypeAttribute>().HasData(
                new SupplyTypeAttribute
                {
                    AttributeName = "TIPO_MEDICAMENTO",
                    AttributeDescription = "Tipo de medicamento",
                    SupplyTypeId = "MEDICAMENTO"
                }
            );
            modelBuilder.Entity<Area>().HasData(
                new Area
                {
                    Name = "ATENCION_A_PACIENTES",
                    Description = "Atención de pacientes"
                },
                new Area
                {
                    Name = "TERAPIA_INTENSIVA",
                    Description = "Terapia Intensiva"
                },
                new Area
                {
                    Name = "TECNICOS",
                    Description = "Técnicos"
                }
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyAttribute> SupplyAttributes { get; set; }
        public DbSet<SupplyType> SupplyTypes { get; set; }
        public DbSet<SupplyTypeAttribute> SupplyTypeAttributes { get; set; }
        public DbSet<SuppliesOrder> SuppliesOrders { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Area> Areas { get; set; }

    }
}