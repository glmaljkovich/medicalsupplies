 
using ArqNetCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArqNetCore.Configuration
{
    public class ArqNetCoreDbContext : DbContext
    {
        public ArqNetCoreDbContext(DbContextOptions<ArqNetCoreDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

    }
}