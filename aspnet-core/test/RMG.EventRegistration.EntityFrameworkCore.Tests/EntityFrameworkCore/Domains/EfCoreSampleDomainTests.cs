using RMG.EventRegistration.Samples;
using Xunit;

namespace RMG.EventRegistration.EntityFrameworkCore.Domains;

[Collection(EventRegistrationTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EventRegistrationEntityFrameworkCoreTestModule>
{

}
