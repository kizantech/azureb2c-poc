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
            modelBuilder.Entity<UserInvitation>().HasData(
                new UserInvitation { InvitationCode = InviteCodeGenerator.GenerateInviteCode(), DelegatedUserManagementRole = Constants.DelegatedUserManagementRoles.CompanyAdmin, CustomerId = Guid.NewGuid(), CreatedBy = "system", CreatedTime = DateTimeOffset.Now, ExpiresTime = DateTimeOffset.Now },
                new UserInvitation { InvitationCode = InviteCodeGenerator.GenerateInviteCode(), DelegatedUserManagementRole = Constants.DelegatedUserManagementRoles.CompanyAdmin, CustomerId = Guid.NewGuid(), CreatedBy = "system", CreatedTime = DateTimeOffset.Now, ExpiresTime = DateTimeOffset.Now }
            );
                        
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
