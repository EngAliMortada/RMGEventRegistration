using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace RMG.EventRegistration.Event;

public class Event : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public int Capacity { get; private set; }
    public bool IsOnline { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public Guid OrganizerId { get; private set; }
    public string Link { get; private set; }
    public string Location { get; private set; }
    public bool IsActive { get; private set; }

    #region navigation
    public virtual IdentityUser? Organizer { get; set; }
    #endregion navigation
}
