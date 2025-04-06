using Volo.Abp.Modularity;

namespace RMG.EventRegistration;

public abstract class EventRegistrationApplicationTestBase<TStartupModule> : EventRegistrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
