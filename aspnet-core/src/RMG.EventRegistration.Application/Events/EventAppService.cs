using Microsoft.Extensions.Logging;
using RMG.EventRegistration.Events.Dtos;
using RMG.EventRegistration.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace RMG.EventRegistration.Events
{
    [RemoteService(false)]
    public class EventAppService : ApplicationService, IEventAppService
    {

        private readonly IEventRepository _eventRepository;
        private readonly IObjectMapper _mapper;
        public EventAppService(IEventRepository eventRepository, IObjectMapper objectMapper)
        {
            _eventRepository = eventRepository;
            _mapper = objectMapper; 
        }

        public async Task<Result<EventDisplayDto>> CreateEventASync(EventCreationDto dto)
        {
            try
            {
                Guid? userId = CurrentUser?.GetId();

                if(!userId.HasValue)
                {
                    return new FailedResult<EventDisplayDto>() { };
                }

                var eventinstanceResult = Event.Create(dto.Name, 
                                                       dto.Capacity, 
                                                       dto.IsOnline, 
                                                       dto.StartDate, 
                                                       dto.EndDate, 
                                                       userId.Value, 
                                                       dto.Link, 
                                                       dto.Location, 
                                                       dto.IsActive);

                if (eventinstanceResult.IsFailed) 
                {
                    Logger.LogDebug("error in {className} in method {methodName} >> error code: {errorCode} >> errorMessage {errorMessage}", 
                                    nameof(EventAppService), 
                                    nameof(CreateEventASync), 
                                    eventinstanceResult.ErrorCode, 
                                    eventinstanceResult.ErrorMessage);

                    return new FailedResult<EventDisplayDto>() {  };
                }

            
                var createdEvent = await _eventRepository.InsertAsync(eventinstanceResult.Value);
                var returnedDto = _mapper.Map<Event, EventDisplayDto>(createdEvent);
                return new SucceededResult<EventDisplayDto>() { Value = returnedDto };
            }

            catch(Exception ex) 
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                nameof(EventAppService),
                nameof(CreateEventASync),
                ex.Message);
                return new FailedResult<EventDisplayDto>() { };
            }
        }
        
        public async Task<Result<EventDisplayDto>> GetEventById(Guid eventId)
        {
            try
            {
                var query = await _eventRepository.GetQueryableAsNoTracking(includeRegistrations: true);

                Event targetEvent = await AsyncExecuter.FirstOrDefaultAsync(query.Where(e => e.Id == eventId));

                if (targetEvent == null) 
                {
                    return new FailedResult<EventDisplayDto>() { };
                }

                var displayDto = _mapper.Map<Event, EventDisplayDto>(targetEvent);
                
                var userId = CurrentUser.GetId();

                //if the organizer requesting he can edit   
                displayDto.UserCanUpdateEvent = userId == targetEvent.OrganizerId;
                displayDto.UserCanDeleteEvent = userId == targetEvent.OrganizerId;
                displayDto.UserCanViewRegisteredUsers = userId == targetEvent.OrganizerId;
                displayDto.UserCanCancelEvent = targetEvent.UserRegistered(userId);
                displayDto.UserCanRegisterInEvent = userId != targetEvent.OrganizerId && 
                                                    CurrentUser.IsInRole(IdentityConstants.Roles.Registrant) && 
                                                    targetEvent.UserNotRegistered(userId) &&
                                                    !targetEvent.MaximumCapacityReached();

                return new SucceededResult<EventDisplayDto>() { Value =  displayDto };  
            }

            catch (Exception ex) 
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                nameof(EventAppService),
                nameof(GetEventById),
                ex.Message);

                return new FailedResult<EventDisplayDto>() { };
            }
        }

        public async Task<ListResult<EventRegistrationDisplayDto>> GetEventRegistrations(Guid eventId)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return new FailedListResult<EventRegistrationDisplayDto>() { };
                }

                Guid userId = CurrentUser.GetId();

                var query = await _eventRepository.GetQueryableIncludeUsers();

                var mappedRegistrations = await AsyncExecuter.FirstOrDefaultAsync(query
                                                                          .Where(e => e.Id == eventId)
                                                                          .Select(e => e.Registrations
                                                                                        .Select(r => new EventRegistrationDisplayDto() { UserEmail = r.User.Email,
                                                                                                                                         UserName = r.User.UserName,
                                                                                                                                         Name = r.User.Name
                                                                                                                                       })
                                                                                  )
                                                                          );
                if (mappedRegistrations == null)
                {
                    return new FailedListResult<EventRegistrationDisplayDto>() { };
                }

                var result = mappedRegistrations.ToList();
                
                return new SucceededListResult<EventRegistrationDisplayDto>() { Values = result };
            }
            catch (Exception ex)
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                    nameof(EventAppService),
                    nameof(GetEventRegistrations),
                    ex.Message);
                        return new FailedListResult<EventRegistrationDisplayDto>() { };
            }

        }

        public async Task<ListResult<EventDisplayDto>> GetAllActiveEvents()
        {

            try
            {
                IQueryable<Event> query = await _eventRepository.GetQueryableAsNoTracking(includeRegistrations: false);

                var result = await AsyncExecuter.ToListAsync(
                    query.Where(e => e.IsActive).Select(e => new EventDisplayDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Capacity = e.Capacity,
                        IsOnline = e.IsOnline,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,    
                        Link = e.Link,
                        Location = e.Location,
                        IsActive = e.IsActive
                    })
                );

                return new SucceededListResult<EventDisplayDto>() { Values = result };
            }
            catch(Exception ex) 
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                        nameof(EventAppService),
                        nameof(GetAllActiveEvents),
                        ex.Message);
                return new FailedListResult<EventDisplayDto> () { };
            }

        }

        public async Task<ListResult<EventDisplayDto>> GetUserEvents()
        {
            try
            {
                if(CurrentUser == null)
                {
                    return new FailedListResult<EventDisplayDto>();
                }

                bool isAdmin = CurrentUser.IsInRole(IdentityConstants.Roles.Admin);
                bool isNormalUser = CurrentUser.IsInRole(IdentityConstants.Roles.Registrant);

                if (isAdmin)
                {
                    return await GetAdminEvents(CurrentUser.GetId());
                }
                else if (isNormalUser) 
                {
                    return await GetRegsitrantEvents(CurrentUser.GetId());
                }

                else return new FailedListResult<EventDisplayDto>();
            }
            catch (Exception ex)
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                        nameof(EventAppService),
                        nameof(GetUserEvents),
                        ex.Message);
                return new FailedListResult<EventDisplayDto>() { };
            }
        }

        public async Task<Result<object>> RegisterInEvent(EventRegistrationDto dto)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return new FailedResult<object>() { };
                }

                Guid userId = CurrentUser.GetId();

                var query = await _eventRepository.WithDetailsAsync(e => e.Registrations);

                Event targetEvent = await AsyncExecuter.FirstOrDefaultAsync(query.Where(e => e.Id == dto.Id));

                if (targetEvent == null) 
                {
                    return new FailedResult<object>() { };
                }

                Result registrationResult = targetEvent.RegisterNewUser(userId);

                if (registrationResult.IsFailed) 
                {
                    return new FailedResult<object>() { };
                }

                await _eventRepository.UpdateAsync(targetEvent);

                return new SucceededResult<object>();

            }
            catch (Exception ex)
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                        nameof(EventAppService),
                        nameof(RegisterInEvent),
                        ex.Message);
                return new FailedResult<object>() { };
            }
        }

        public async Task<Result<object>> CancelRegistratonDto(EventRegistrationCancelDto dto)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return new FailedResult<object>() { };
                }

                Guid userId = CurrentUser.GetId();

                var query = await _eventRepository.WithDetailsAsync(e => e.Registrations);

                Event targetEvent = await AsyncExecuter.FirstAsync(query.Where(e => e.Id == dto.EventId));

                Result cancelationResult = targetEvent.CancelUserRegistration(userId);

                if (cancelationResult.IsFailed)
                {
                    return new FailedResult<object>() { };
                }

                await _eventRepository.UpdateAsync(targetEvent);

                return new SucceededResult<object>();

            }
            catch (Exception ex)
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                        nameof(EventAppService),
                        nameof(CancelRegistratonDto),
                        ex.Message);
                return new FailedResult<object>() { };
            }
        }
    
        public async Task<Result<EventDisplayDto>> UpdateEventAsync(EventUpdateDto dto)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return new FailedResult<EventDisplayDto>() { };
                }

                Guid userId = CurrentUser.GetId();

                var query = await _eventRepository.WithDetailsAsync(e => e.Registrations);

                Event targetEvent = await AsyncExecuter.FirstAsync(query.Where(e => e.Id == dto.Id));

                //admin can only edit his own events
                if (targetEvent.OrganizerId != userId) 
                {
                    return new FailedResult<EventDisplayDto>() { };
                }

                Result updateDomainEntityResult = targetEvent.Update(dto.Name, dto.Capacity, dto.IsOnline, dto.StartDate, dto.EndDate, dto.Link, dto.Location, dto.IsActive);

                if (updateDomainEntityResult.IsFailed)
                {
                    return new FailedResult<EventDisplayDto>() { };
                }

                var updatedEvent = await _eventRepository.UpdateAsync(targetEvent);

                EventDisplayDto displayDto = _mapper.Map<Event, EventDisplayDto>(updatedEvent);

                return new SucceededResult<EventDisplayDto>() { Value = displayDto };
            }
            catch (Exception ex) 
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                                nameof(EventAppService),
                                nameof(UpdateEventAsync),
                                ex.Message);

                return new FailedResult<EventDisplayDto>() { };
            }
        }

        public async Task<Result<object>> DeleteEventAsync(EventDeletionDto dto)
        {
            try
            {
                if (CurrentUser == null)
                {
                    return new FailedResult<object>() { };
                }

                Guid userId = CurrentUser.GetId();

                var query = await _eventRepository.WithDetailsAsync(e => e.Registrations);

                Event targetEvent = await AsyncExecuter.FirstAsync(query.Where(e => e.Id == dto.Id));

                //admin can only delete his own events
                if (targetEvent.OrganizerId != userId)
                {
                    return new FailedResult<object>() { };
                }

                Result deleteDomainEntityResult = targetEvent.MarkAsDeleted();

                if (deleteDomainEntityResult.IsFailed)
                {
                    return new FailedResult<object>() { };
                }

                var updatedEvent = await _eventRepository.UpdateAsync(targetEvent);


                return new SucceededResult<object>();
            }
            catch (Exception ex)
            {
                Logger.LogError("error in {className} in method {methodName} >> errorMessage {errorMessage}",
                                nameof(EventAppService),
                                nameof(DeleteEventAsync),
                                ex.Message);

                return new FailedResult<object>() { };
            }
        }

        #region private
        private async Task<ListResult<EventDisplayDto>> GetAdminEvents(Guid id)
        {
            IQueryable<Event> query = await _eventRepository.GetQueryableAsNoTracking(includeRegistrations: false);

            var result = await AsyncExecuter.ToListAsync(
                query.Where(e => e.OrganizerId == id).Select(e => new EventDisplayDto()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Capacity = e.Capacity,
                    IsOnline = e.IsOnline,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Link = e.Link,
                    Location = e.Location,
                    IsActive = e.IsActive
                })
            );

            return new SucceededListResult<EventDisplayDto>() { Values = result };
        }

        private async Task<ListResult<EventDisplayDto>> GetRegsitrantEvents(Guid id)
        {
            IQueryable<Event> query = await _eventRepository.GetQueryableAsNoTracking(includeRegistrations: true);

            var result = await AsyncExecuter.ToListAsync(
                                query
                                .Where(e => e.Registrations != null && e.Registrations.Any(registration => registration.UserId == id))
                                
                                .Select(e => new EventDisplayDto()
                                {
                                    Id = e.Id,
                                    Name = e.Name,
                                    Capacity = e.Capacity,
                                    IsOnline = e.IsOnline,
                                    StartDate = e.StartDate,
                                    EndDate = e.EndDate,
                                    Link = e.Link,
                                    Location = e.Location,
                                    IsActive = e.IsActive
                                })
                            );

            return new SucceededListResult<EventDisplayDto>() { Values = result };
        }
        #endregion private

    }
}
