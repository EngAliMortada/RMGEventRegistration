using RMG.EventRegistration.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace RMG.EventRegistration.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EventRegistrationEntityFrameworkCoreModule),
    typeof(EventRegistrationApplicationContractsModule)
    )]
public class EventRegistrationDbMigratorModule : AbpModule
{
}
