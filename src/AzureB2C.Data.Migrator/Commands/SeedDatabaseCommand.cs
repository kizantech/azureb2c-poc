using AzureB2C.Data.Context;

namespace AzureB2C.Data.Migrator.Commands;

public class SeedDatabaseCommand
{
    private readonly CustomerDbContext _dbContext;
    private readonly AzureB2cAuthDbContext _authDbContext;

    public SeedDatabaseCommand(CustomerDbContext dbContext, AzureB2cAuthDbContext authDbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _authDbContext = authDbContext ?? throw new ArgumentNullException(nameof(authDbContext));
    }

    public async Task Execute()
    {
        await Task.Delay(200);
    }
}