using System;
using Volo.Abp.Application.Dtos;

namespace RMG.EventRegistration.Events.Dtos
{
    public class EventDisplayDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsOnline { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Link { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }
        public bool UserCanUpdateEvent { get; set; }    
        public bool UserCanDeleteEvent { get; set; }
        public bool UserCanCancelEvent { get; set ; }
        public bool UserCanRegisterInEvent { get; set; }
        public bool UserCanViewRegisteredUsers { get; set; }
    }
}
