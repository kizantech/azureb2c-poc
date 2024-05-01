using System.Security.Cryptography;
using AzureB2C.Data.Context;
using AzureB2C.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureB2C.Data.Migrator;

public static class DatabaseServiceCollectionExtension
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var baseTenant = new CustomerInfo()
        {
            Id = "0",
            Identifier = Guid.Empty.ToString(),
            Name = "Base Tenant",
            CustomerId = Guid.Empty,
            ConnectionString = null
        };
        services.AddSingleton<CustomerInfo>(baseTenant);
        services.AddDbContext<CustomerDbContext>((_, options) =>
        {
            options.ConfigureDbContext(configuration);
        });
        services.AddDbContext<AzureB2cAuthDbContext>((_, options) =>
        {
            options.ConfigureDbContext(configuration);
        });
        services.AddScoped<DbContext>(x => x.GetRequiredService<CustomerDbContext>());
        services.AddScoped<DbContext>(x => x.GetRequiredService<AzureB2cAuthDbContext>());
    }

    public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder builder,
        IConfiguration configuration)
    {
        var connectionString = GetSqlServerConnectionString(configuration);
        return builder.UseSqlServer(connectionString, x =>
        {
            x.MigrationsAssembly("AzureB2C.Data");
        });
    }

    public static string GetSqlServerConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDatabase");
        if (connectionString == null) throw new Exception("Missing SqlServer connection string.");
        return connectionString;
    }
}