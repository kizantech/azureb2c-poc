using AzureB2C.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AzureB2C.Data.Context;

public class AzureB2cAuthDbContextFactory : IDesignTimeDbContextFactory<AzureB2cAuthDbContext>
{
    public AzureB2cAuthDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddUserSecrets<AzureB2cAuthDbContextFactory>()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<AzureB2cAuthDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityDatabase"));
        return new AzureB2cAuthDbContext(optionsBuilder.Options);
    }
}