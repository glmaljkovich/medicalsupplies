 
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
            modelBuilder.Entity<OrganizationSupplyType>()
                .HasKey(supplyTypeAttribute => new { 
                    supplyTypeAttribute.SupplyTypeId, 
                    supplyTypeAttribute.OrganizationId
                });   
            string MASCARA_PROTECTORA_ID = "MASCARA_PROTECTORA";
            string BARBIJO_ID = "BARBIJO";
            string RESPIRADOR_ID = "RESPIRADOR";
            string GUANTE_ID = "GUANTE";
            string MEDICAMENTO_ID = "MEDICAMENTO";
            modelBuilder.Entity<SupplyType>().HasData(
                new SupplyType
                {
                    Id = MASCARA_PROTECTORA_ID,
                    Description = "Máscara protectora"
                },
                new SupplyType
                {
                    Id = BARBIJO_ID,
                    Description = "Barbijo"
                },
                new SupplyType
                {
                    Id = RESPIRADOR_ID,
                    Description = "Respirador"
                },
                new SupplyType
                {
                    Id = GUANTE_ID,
                    Description = "Guante"
                },
                new SupplyType
                {
                    Id = MEDICAMENTO_ID,
                    Description = "Medicamento"
                }
            );
            modelBuilder.Entity<SupplyTypeAttribute>().HasData(
                new SupplyTypeAttribute
                {
                    AttributeName = "TIPO_MEDICAMENTO",
                    AttributeDescription = "Tipo de medicamento",
                    SupplyTypeId = MEDICAMENTO_ID
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
            int ORGANIZATION_INSUMOS_BASICOS_ID = 1;
            int ORGANIZATION_INSUMOS_MECANICOS_ID = 2;
            int ORGANIZATION_MEDICAMENTOS_ID = 3;
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = ORGANIZATION_INSUMOS_BASICOS_ID,
                    Name = "Insumos Basicos"
                },
                new Organization
                {
                    Id = ORGANIZATION_INSUMOS_MECANICOS_ID,
                    Name = "Insumos mecanicos"
                },
                new Organization
                {
                    Id = ORGANIZATION_MEDICAMENTOS_ID,
                    Name = "Medicamentos"
                }
            );
            modelBuilder.Entity<OrganizationSupplyType>().HasData(
                new OrganizationSupplyType
                {
                    OrganizationId = ORGANIZATION_INSUMOS_BASICOS_ID,
                    SupplyTypeId = BARBIJO_ID
                },
                new OrganizationSupplyType
                {
                    OrganizationId = ORGANIZATION_INSUMOS_BASICOS_ID,
                    SupplyTypeId = GUANTE_ID
                },
                new OrganizationSupplyType
                {
                    OrganizationId = ORGANIZATION_INSUMOS_BASICOS_ID,
                    SupplyTypeId = MASCARA_PROTECTORA_ID
                },
                new OrganizationSupplyType
                {
                    OrganizationId = ORGANIZATION_INSUMOS_MECANICOS_ID,
                    SupplyTypeId = RESPIRADOR_ID
                },
                new OrganizationSupplyType
                {
                    OrganizationId = ORGANIZATION_MEDICAMENTOS_ID,
                    SupplyTypeId = MEDICAMENTO_ID
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
        public DbSet<OrganizationSupplyType> OrganizationSupplyTypes { get; set; }
        public DbSet<Area> Areas { get; set; }

    }
}