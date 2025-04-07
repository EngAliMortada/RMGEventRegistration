using System;
using System.ComponentModel.DataAnnotations;
using RMG.EventRegistration.Events;

namespace RMG.EventRegistration.Events.Dtos
{
    public class EventCreationDto
    {
        [Required]
        [MaxLength(EventValidationConstants.NameMaxLength)]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsOnline { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset EndDate { get; set; }
        public string? Link { get; set; }

        [MaxLength(EventValidationConstants.LocationMaxLength)]
        public string? Location { get; set; }
        public bool IsActive { get; set; }
    }
}
