using System;

namespace RMG.EventRegistration.Events.Dtos
{
    public class EventUpdateDto : EventCreationDto
    {
        public Guid Id { get; set; }
    }
}
