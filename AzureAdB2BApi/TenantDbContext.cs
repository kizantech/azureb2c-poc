using Microsoft.EntityFrameworkCore;
using AzureAdB2BApi.Models;
using System.Reflection.Emit;

namespace AzureAdB2BApi
{
    public class TenantDbContext: DbContext
    {
        public TenantDbContext(){}
        public TenantDbContext(DbContextOptions options): base(options){}
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().HasData(
                new Tenant{Id = "123", ProviderName = "google.com"},
                new Tenant{ Id = "456", ProviderName = "live.com" }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("apiDb");
        }
    }
}
