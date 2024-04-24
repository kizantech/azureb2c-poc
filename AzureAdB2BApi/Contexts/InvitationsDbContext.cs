using Microsoft.EntityFrameworkCore;
using AzureAdB2BApi.Models;
using System.Reflection.Emit;
using AzureAdB2BApi.Utils;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;

namespace AzureAdB2BApi.Contexts
{
    public class InvitationsDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public InvitationsDbContext(DbContextOptions options) : base(options) { }
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
