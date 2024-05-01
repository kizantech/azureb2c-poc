using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureB2C.Data.Migrator.Commands;

public class MigrateDatabaseCommand
{
    private readonly IEnumerable<DbContext> _dbContexts;
    private readonly ILogger<MigrateDatabaseCommand> _logger;

    public MigrateDatabaseCommand(IEnumerable<DbContext> dbContexts, ILogger<MigrateDatabaseCommand> logger)
    {
        _dbContexts = dbContexts ?? throw new ArgumentNullException(nameof(dbContexts));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Execute()
    {
        foreach (var dbContext in _dbContexts)
        {
            var dbContextName = dbContext.GetType().Name;
            _logger.LogInformation("Migrating {DbContextName}...", dbContextName);
            await dbContext.Database.MigrateAsync();
            _logger.LogInformation("Migration of {DbContextName} completed.", dbContextName);
        }
    }
}