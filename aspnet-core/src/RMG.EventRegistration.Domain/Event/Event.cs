using RMG.EventRegistration.Extensions;
using RMG.EventRegistration.Results;
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

    private Event(string name,
                    DateTimeOffset startDate,
                    DateTimeOffset endDate,
                    Guid organizerId,
                    string link,
                    bool isActive)
    {
        Name = name;
        IsOnline = true;
        StartDate = startDate;
        EndDate = endDate;
        OrganizerId = organizerId;
        Link = link;
        IsActive = isActive;
    }

    private Event(string name,
                    int capacity,
                    DateTimeOffset startDate,
                    DateTimeOffset endDate,
                    Guid organizerId,
                    string location,
                    bool isActive) 
    { 
        Name = name;
        IsOnline = false; 
        StartDate = startDate; 
        EndDate = endDate;
        OrganizerId = organizerId;
        Location = location;
        IsActive = isActive;
    }

    protected Event()
    {

    }

    public static Result<Event> Create(string name,
                                        int capacity,
                                        bool isOnline,
                                        DateTimeOffset startDate,
                                        DateTimeOffset endDate,
                                        Guid organizerId,
                                        string link,
                                        string location,    
                                        bool isActive)
    {
        if (isOnline)
        {
            return Event.CreateOnlineEvent(name,startDate, endDate, organizerId, link, isActive);
        }

        return Event.CreateRealLifeEvent(name, capacity, startDate, endDate, organizerId, location, isActive);
    }


    private static Result<Event> CreateOnlineEvent(string name, 
                                                   DateTimeOffset startDate, 
                                                   DateTimeOffset endDate, 
                                                   Guid organizerId, 
                                                   string link, 
                                                   bool isActive)
    {
        Event eventItem = new Event(name, startDate, endDate, organizerId, link, isActive);
        var validationResult = eventItem.Validate();
        if(validationResult.IsFailed)
        {
            return new FailedResult<Event>() { ErrorCode = validationResult.ErrorCode, ErrorMessage = validationResult.ErrorMessage };
        }

        return new SucceededResult<Event>() { Value = eventItem };
    }

    private static Result<Event> CreateRealLifeEvent(string name,
                    int capacity,
                    DateTimeOffset startDate,
                    DateTimeOffset endDate,
                    Guid organizerId,
                    string location,
                    bool isActive)
    {
        Event eventItem = new Event(name, capacity, startDate, endDate, organizerId, location, isActive);
        var validationResult = eventItem.Validate();
        if (validationResult.IsFailed)
        {
            return new FailedResult<Event>() { ErrorCode = validationResult.ErrorCode, ErrorMessage = validationResult.ErrorMessage };
        }

        return new SucceededResult<Event>() { Value = eventItem };
    }


    private Result Validate()
    {
        //Business valudation
        if (Name.IsEmpty())
        {
            return new FailedRequiredResult(nameof(Event.Name));
        }

        if (StartDate.Date < DateTimeOffset.UtcNow.Date)
        {
            return new FailedInvalidParamResult(nameof(Event.StartDate));
        }

        if (EndDate.UtcDateTime < StartDate.UtcDateTime)
        {
            return new FailedInvalidParamResult(nameof(Event.EndDate));
        }

        //validation for online events
        if (IsOnline)
        {
            if (Link.IsEmpty())
            {
                return new FailedRequiredResult(nameof(Event.Link));
            }
        }

        //validate for real life
        else
        {
            if (Capacity < EventValidationConstants.EventCapacityMinValue)
            {
                return new FailedInvalidParamResult(nameof(Event.Capacity));
            }

            if (Location.IsEmpty())
            {
                return new FailedRequiredResult(nameof(Event.Location));
            }
        }


        return new SucceededResult() { };
    }
}
