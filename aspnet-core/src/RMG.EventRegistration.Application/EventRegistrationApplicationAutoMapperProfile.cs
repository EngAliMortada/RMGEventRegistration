﻿using AutoMapper;
using RMG.EventRegistration.Events;
using RMG.EventRegistration.Events.Dtos;

namespace RMG.EventRegistration;

public class EventRegistrationApplicationAutoMapperProfile : Profile
{
    public EventRegistrationApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Event, EventDisplayDto>();
    }
}
