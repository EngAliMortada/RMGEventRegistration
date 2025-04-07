using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMG.EventRegistration.Events;
using RMG.EventRegistration.Events.Dtos;
using RMG.EventRegistration.Results;
using System;
using System.Threading.Tasks;

namespace RMG.EventRegistration.Controllers.Events
{
    [Route("api/events")]
    public class EventsController : EventRegistrationController
    {
        private readonly IEventAppService _appService;

        public EventsController(IEventAppService appService)
        {
            _appService = appService;
        }

        [Route("create")]
        [HttpPost]
        [Authorize(Roles = $"{IdentityConstants.Roles.Admin}")]
        public async Task<Result<EventDisplayDto>> CreateEventASync([FromBody] EventCreationDto dto)
        {
            return await _appService.CreateEventASync(dto);
        }

        [Route("get-active-events")]
        [HttpGet]
        [Authorize(Roles = $"{IdentityConstants.Roles.Registrant},{IdentityConstants.Roles.Admin}")]
        public async Task<ListResult<EventDisplayDto>> GetAllActiveEvents()
        {
            return await _appService.GetAllActiveEvents();
        }

        [Route("get-user-events")]
        [HttpGet]
        [Authorize(Roles = $"{IdentityConstants.Roles.Registrant},{IdentityConstants.Roles.Admin}")]

        public async Task<ListResult<EventDisplayDto>> GetUserEvents()
        {
            return await _appService.GetUserEvents();
        }

        [Route("register-in-event")]
        [HttpPost]
        [Authorize(Roles = $"{IdentityConstants.Roles.Registrant}")]
        public async Task<Result<object>> RegisterInEvent([FromBody] EventRegistrationDto dto)
        {
            return await _appService.RegisterInEvent(dto);
        }

        [Route("cancel-registraion")]
        [HttpPost]
        [Authorize(Roles = $"{IdentityConstants.Roles.Registrant}")]
        public async Task<Result<object>> CancelRegistratonDto([FromBody] EventRegistrationCancelDto dto)
        {
            return await _appService.CancelRegistratonDto(dto);
        }

        [Route("update-event")]
        [HttpPost]
        [Authorize(Roles = $"{IdentityConstants.Roles.Admin}")]

        public async Task<Result<EventDisplayDto>> UpdateEventAsync(EventUpdateDto dto)
        {
            return await _appService.UpdateEventAsync(dto);
        }

        [Route("delete-event")]
        [HttpPost]
        [Authorize(Roles = $"{IdentityConstants.Roles.Admin}")]
        public async Task<Result<object>> DeleteEventAsync(EventDeletionDto dto)
        {
            return await _appService.DeleteEventAsync(dto);
        }

        [HttpGet("get-event-registrations/{eventId}")]
        [Authorize(Roles = $"{IdentityConstants.Roles.Admin}")]
        public async Task<ListResult<EventRegistrationDisplayDto>> GetEventRegistrations(Guid eventId)
        {
            return await _appService.GetEventRegistrations(eventId);
        }

        [HttpGet("get-event-byid/{eventId}")]
        [Authorize(Roles = $"{IdentityConstants.Roles.Registrant},{IdentityConstants.Roles.Admin}")]
        public async Task<Result<EventDisplayDto>> GetEventById(Guid eventId)
        {
            return await _appService.GetEventById(eventId);
        }
    }
}
