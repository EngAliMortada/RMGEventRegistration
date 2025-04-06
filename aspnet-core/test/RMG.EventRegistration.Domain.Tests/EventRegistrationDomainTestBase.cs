using Volo.Abp.Modularity;

namespace RMG.EventRegistration;

/* Inherit from this class for your domain layer tests. */
public abstract class EventRegistrationDomainTestBase<TStartupModule> : EventRegistrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
