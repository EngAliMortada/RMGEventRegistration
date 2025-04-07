using System;

namespace RMG.EventRegistration.Events.Dtos
{
    public class EventRegistrationDisplayDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
    }
}
