using System.Reflection;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using TenantInfo = AzureAdB2BApi.Models.TenantInfo;

namespace BlazorAppPoc.Contexts;

public class PocDbContextFactory : IDesignTimeDbContextFactory<PocDbContext>
{
    public PocDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddUserSecrets<PocDbContextFactory>()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<PocDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityDatabase"));
        var baseTenant = new TenantInfo()
        {
            Id = "0",
            Identifier = Guid.Empty.ToString(),
            Name = "Base Tenant",
            CustomerId = Guid.Empty,
            ConnectionString = null
        };
        return new PocDbContext(baseTenant, optionsBuilder.Options);
    }
}