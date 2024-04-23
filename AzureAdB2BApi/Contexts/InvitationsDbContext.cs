using Microsoft.EntityFrameworkCore;
using AzureAdB2BApi.Models;
using System.Reflection.Emit;
using AzureAdB2BApi.Utils;

namespace AzureAdB2BApi.Contexts
{
    public class InvitationsDbContext : DbContext
    {
        public InvitationsDbContext() { }
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
            optionsBuilder.UseInMemoryDatabase("apiDb");
        }
    }
}
