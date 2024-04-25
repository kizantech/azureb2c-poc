using AzureAdB2BApi.Models;
using BlazorAppPoc.Models;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppPoc.Contexts
{
    public class PocDbContext : MultiTenantIdentityDbContext
    {
        private TenantInfo TenantInfo { get; set; }

        public PocDbContext(TenantInfo tenantInfo) : base(tenantInfo)
        {
            TenantInfo = tenantInfo;
        }

        public PocDbContext(TenantInfo tenantInfo, DbContextOptions<PocDbContext> options) : base(tenantInfo, options)
        {
            TenantInfo = tenantInfo;
        }
        
        public DbSet<ToDo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (TenantInfo?.ConnectionString != null)
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
        }
    }
}
