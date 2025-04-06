using RMG.EventRegistration.Samples;
using Xunit;

namespace RMG.EventRegistration.EntityFrameworkCore.Applications;

[Collection(EventRegistrationTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EventRegistrationEntityFrameworkCoreTestModule>
{

}
