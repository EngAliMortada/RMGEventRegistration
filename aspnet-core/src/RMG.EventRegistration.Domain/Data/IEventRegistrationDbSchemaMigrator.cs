using System.Threading.Tasks;

namespace RMG.EventRegistration.Data;

public interface IEventRegistrationDbSchemaMigrator
{
    Task MigrateAsync();
}
