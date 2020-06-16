 
using ArqNetCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArqNetCore.Configuration
{
    public class ArqNetCoreDbContext : DbContext
    {
        public ArqNetCoreDbContext(DbContextOptions<ArqNetCoreDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyOption> SupplyOptions { get; set; }
        public DbSet<SuppliesOrder> SuppliesOrders { get; set; }
        public DbSet<Area> Areas { get; set; }

    }
}