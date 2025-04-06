using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace RMG.EventRegistration.Event;

public class EventRegistration : AuditedEntity<Guid>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    #region navigation properties
    public virtual Event Event { get; protected set; }
    public virtual IdentityUser User { get; protected set; }
    #endregion navigation properties

    private EventRegistration(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
    }

    protected EventRegistration()
    {

    }

    public static EventRegistration Create(Guid eventId, Guid userId)
    {
        // no validation required
        return new EventRegistration(eventId, userId);
    }
}
