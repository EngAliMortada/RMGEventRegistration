using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RMG.EventRegistration.Data;
using Volo.Abp.DependencyInjection;

namespace RMG.EventRegistration.EntityFrameworkCore;

public class EntityFrameworkCoreEventRegistrationDbSchemaMigrator
    : IEventRegistrationDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEventRegistrationDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the EventRegistrationDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EventRegistrationDbContext>()
            .Database
            .MigrateAsync();
    }
}
