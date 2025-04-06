using Xunit;

namespace RMG.EventRegistration.EntityFrameworkCore;

[CollectionDefinition(EventRegistrationTestConsts.CollectionDefinitionName)]
public class EventRegistrationEntityFrameworkCoreCollection : ICollectionFixture<EventRegistrationEntityFrameworkCoreFixture>
{

}
