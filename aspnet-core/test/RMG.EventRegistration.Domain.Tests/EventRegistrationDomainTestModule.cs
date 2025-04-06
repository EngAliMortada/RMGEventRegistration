using Volo.Abp.Modularity;

namespace RMG.EventRegistration;

[DependsOn(
    typeof(EventRegistrationDomainModule),
    typeof(EventRegistrationTestBaseModule)
)]
public class EventRegistrationDomainTestModule : AbpModule
{

}
