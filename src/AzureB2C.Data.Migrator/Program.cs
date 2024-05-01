using System.CommandLine;
using AzureB2C.Data.Migrator.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureB2C.Data.Migrator;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration(options =>
        {
            options
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
        });

        builder.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });

        builder.ConfigureServices((ctx, services) =>
        {
            services.AddScoped<MigrateDatabaseCommand>();
            services.AddScoped<ClearDatabaseCommand>();
            services.AddScoped<SeedDatabaseCommand>();
            services.AddDatabase(ctx.Configuration);
        });

        IHost app = builder.Build();
        var hostEnv = app.Services.GetRequiredService<IHostEnvironment>();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Running in {hostEnv} Mode", hostEnv.EnvironmentName);

        await using var scope = app.Services.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;
        var rootCommand = new RootCommand("AzureB2C.Data.Migrator.Commands");
        AddMigrateDatabaseCommand(rootCommand, serviceProvider);
        AddClearDatabaseCommand(rootCommand, serviceProvider);
        AddSeedDatabaseCommand(rootCommand, serviceProvider);

        await rootCommand.InvokeAsync(args);
    }

    static void AddMigrateDatabaseCommand(RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var command = new Command("migrate", "Migrates the database.");
        command.SetHandler(() =>
        {
            var migrateDbCommand = serviceProvider.GetRequiredService<MigrateDatabaseCommand>();
            return migrateDbCommand.Execute();
        });
        rootCommand.Add(command);
    }

    static void AddClearDatabaseCommand(RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var command = new Command("clear", "Clears all tables and resets id counter, foreign keys etc");
        command.SetHandler(() => serviceProvider.GetRequiredService<ClearDatabaseCommand>().Execute());
        rootCommand.Add(command);
    }

    static void AddSeedDatabaseCommand(RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var command = new Command("seed", "Seeds database with pre-defined data");
        command.SetHandler(() => serviceProvider.GetRequiredService<SeedDatabaseCommand>().Execute());
        rootCommand.Add(command);
    }
}