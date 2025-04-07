using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace RMG.EventRegistration.Events;

public class Registration : Entity<Guid>, ISoftDelete
{
    public Guid EventId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    public DateTimeOffset RegistrationDate { get; set; }

    #region navigation properties
    public virtual Event Event { get; protected set; }
    public virtual IdentityUser User { get; protected set; }
    #endregion navigation properties

    private Registration(Guid eventId, Guid userId)
    {
        EventId = eventId;
        UserId = userId;
        RegistrationDate = DateTimeOffset.UtcNow;
    }

    protected Registration()
    {

    }

    public static Registration Create(Guid eventId, Guid userId)
    {
        // no validation required
        return new Registration(eventId, userId);
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}
