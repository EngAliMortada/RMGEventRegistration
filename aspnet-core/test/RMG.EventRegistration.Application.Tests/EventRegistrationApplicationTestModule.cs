using Volo.Abp.Modularity;

namespace RMG.EventRegistration;

[DependsOn(
    typeof(EventRegistrationApplicationModule),
    typeof(EventRegistrationDomainTestModule)
)]
public class EventRegistrationApplicationTestModule : AbpModule
{

}
