using RMG.EventRegistration.Events.Dtos;
using RMG.EventRegistration.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace RMG.EventRegistration.Events
{
    public interface IEventAppService : IApplicationService
    {
        Task<Result<EventDisplayDto>> CreateEventASync(EventCreationDto dto);
        Task<ListResult<EventDisplayDto>> GetAllActiveEvents();
        /// <summary>
        /// if user has admin role >> return events he created
        /// if normal user >> return events he is registered in
        /// </summary>
        Task<ListResult<EventDisplayDto>> GetUserEvents();
        Task<Result<object>> RegisterInEvent(EventRegistrationDto dto);
        Task<Result<object>> CancelRegistratonDto(EventRegistrationCancelDto dto);
        Task<Result<EventDisplayDto>> UpdateEventAsync(EventUpdateDto dto);
        Task<Result<object>> DeleteEventAsync(EventDeletionDto dto);
        Task<ListResult<EventRegistrationDisplayDto>> GetEventRegistrations(Guid eventId);
        Task<Result<EventDisplayDto>> GetEventById(Guid eventId);
    }
}
