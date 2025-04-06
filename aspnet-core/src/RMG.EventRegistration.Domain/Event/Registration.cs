using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace RMG.EventRegistration.Events;

public class Registration : FullAuditedEntity<Guid>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    #region navigation properties
    public virtual Event Event { get; protected set; }
    public virtual IdentityUser User { get; protected set; }
    #endregion navigation properties

    private Registration(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
    }

    protected Registration()
    {

    }

    public static Registration Create(Guid eventId, Guid userId)
    {
        // no validation required
        return new Registration(eventId, userId);
    }
}
