using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RMG.EventRegistration.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EventRegistrationDbContextFactory : IDesignTimeDbContextFactory<EventRegistrationDbContext>
{
    public EventRegistrationDbContext CreateDbContext(string[] args)
    {
        EventRegistrationEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<EventRegistrationDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new EventRegistrationDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RMG.EventRegistration.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
