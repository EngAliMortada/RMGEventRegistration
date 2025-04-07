using RMG.EventRegistration.Extensions;
using RMG.EventRegistration.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace RMG.EventRegistration.Events;

public class Event : AggregateRoot<Guid>, ISoftDelete
{
    [MaxLength(EventValidationConstants.NameMaxLength)]
    public string Name { get; private set; }
    public int Capacity { get; private set; }
    public bool IsOnline { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public Guid OrganizerId { get; private set; }
    public string? Link { get; private set; }

    [MaxLength(EventValidationConstants.LocationMaxLength)]
    public string? Location { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; } = false;
    #region navigation
    public virtual IdentityUser? Organizer { get; set; }
    public virtual ICollection<Registration> Registrations { get; set; }
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
        Capacity = capacity;
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
                                        string? link,
                                        string? location,    
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

        if(Name.Length > EventValidationConstants.NameMaxLength)
        {
            return new FailedInvalidParamResult(nameof(Event.Name));
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

            if (Location.Length > EventValidationConstants.LocationMaxLength)
            {
                return new FailedInvalidParamResult(nameof(Event.Location));
            }
        }


        return new SucceededResult() { };
    }

    public Result RegisterNewUser(Guid registrantId)
    {
        if(Registrations == null)
        {
            return new FailedNullResult(nameof(Registrations));
        }

        //check if user already registered
        if (Registrations.Any(r => r.UserId == registrantId))
        {
            return new FailedResult() { ErrorMessage = "User Already registered" };
        }

        if(DateTimeOffset.UtcNow.DateTime >=  StartDate.DateTime)
        {
            return new FailedResult() { ErrorMessage = "Event no longer available" };
        }

        //validate capacity
        if (ShouldValidateCapacity() && MaximumCapacityReached())
        {
            return new FailedResult() { ErrorMessage = "event capacity is full" };
        }

        //create registration record
        Registration registration = Registration.Create(Id, registrantId);
        Registrations.Add(registration);
        return new SucceededResult();
    }

    public bool UserRegistered(Guid userId)
    {
        if (Registrations == null) return false;
        return Registrations.Any(r => r.UserId == userId);  
    }

    public bool UserNotRegistered(Guid userId)
    {
        return !UserRegistered(userId);

    }

    public Result CancelUserRegistration(Guid registrantId)
    {
        if (Registrations == null)
        {
            return new FailedNullResult(nameof(Registrations));
        }

        //check if user already canceld
        if (!Registrations.Any(r => r.UserId == registrantId))
        {
            return new FailedResult() { ErrorMessage = "User already not registered" };
        }

        //can cancel registration anytime until 1 hour before event’s start date
        if (DateTimeOffset.UtcNow.DateTime >= (StartDate.DateTime.AddHours(-1)))
        {
            return new FailedResult() { ErrorMessage = "cancelation no longer available" };
        }

        Registration toBeDeleted = Registrations.First(r => r.UserId == registrantId);

        toBeDeleted.MarkAsDeleted();

        return new SucceededResult();
    }

    public Result Update(string name,
                        int capacity,
                        bool isOnline,
                        DateTimeOffset startDate,
                        DateTimeOffset endDate,
                        string? link,
                        string? location,
                        bool isActive)
    {
        //validate the input by creating a new validated instance
        //we pass OrganizerId as is, because it couldn't change
        var validatedEventCreationResult = Event.Create(name, capacity, isOnline, startDate, endDate, OrganizerId, link, location, isActive);
        if(validatedEventCreationResult.IsFailed)
        {
            return new FailedResult() { ErrorCode = validatedEventCreationResult.ErrorCode, ErrorMessage = validatedEventCreationResult.ErrorMessage };
        }
        Event updatedInstance = validatedEventCreationResult.Value;

        //perform updating from the valid instance
        Name = updatedInstance.Name;
        Capacity = updatedInstance.Capacity;
        IsOnline = updatedInstance.IsOnline;
        StartDate = updatedInstance.StartDate;
        EndDate = updatedInstance.EndDate;
        Link = updatedInstance.Link;
        Location = updatedInstance.Location;
        IsActive = updatedInstance.IsActive;
        return new SucceededResult();
    }

    public Result MarkAsDeleted()
    {
        IsDeleted = true;
        if(Registrations == null)
        {
            return new FailedResult();
        }

        foreach (Registration r in Registrations) 
        { 
            r.MarkAsDeleted();
        }

        return new SucceededResult();
    }

    private bool ShouldValidateCapacity()
    {
        //validate capacity in real life events only
        return !IsOnline;
    }

    public bool MaximumCapacityReached()
    {
        return Registrations != null && Registrations.Count == Capacity;
    }
}
