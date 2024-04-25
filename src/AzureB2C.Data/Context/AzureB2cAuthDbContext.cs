using AzureB2C.Data.Model;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.EntityFrameworkCore;

namespace AzureB2C.Data.Context
{
    public class AzureB2cAuthDbContext : EFCoreStoreDbContext<CustomerInfo>
    {
        public AzureB2cAuthDbContext(DbContextOptions options) : base(options) { }
        public DbSet<UserInvitation> UserInvitations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
