using AzureB2C.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AzureB2C.Data.Context;

public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddUserSecrets<CustomerDbContextFactory>()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityDatabase"));
        var baseTenant = new CustomerInfo()
        {
            Id = "0",
            Identifier = Guid.Empty.ToString(),
            Name = "Base Tenant",
            CustomerId = Guid.Empty,
            ConnectionString = null
        };
        return new CustomerDbContext(baseTenant, optionsBuilder.Options);
    }
}