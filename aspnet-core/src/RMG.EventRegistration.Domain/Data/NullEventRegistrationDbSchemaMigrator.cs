using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace RMG.EventRegistration.Data;

/* This is used if database provider does't define
 * IEventRegistrationDbSchemaMigrator implementation.
 */
public class NullEventRegistrationDbSchemaMigrator : IEventRegistrationDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
