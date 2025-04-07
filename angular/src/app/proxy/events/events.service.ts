import type { EventCreationDto, EventDeletionDto, EventDisplayDto, EventRegistrationCancelDto, EventRegistrationDisplayDto, EventRegistrationDto, EventUpdateDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ListResult, Result } from '../results/models';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  apiName = 'Default';
  

  cancelRegistratonDtoByDto = (dto: EventRegistrationCancelDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<object>>({
      method: 'POST',
      url: '/api/events/cancel-registraion',
      body: dto,
    },
    { apiName: this.apiName,...config });
  

  createEventASyncByDto = (dto: EventCreationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<EventDisplayDto>>({
      method: 'POST',
      url: '/api/events/create',
      body: dto,
    },
    { apiName: this.apiName,...config });
  

  deleteEvent = (dto: EventDeletionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<object>>({
      method: 'POST',
      url: '/api/events/delete-event',
      params: { id: dto.id },
    },
    { apiName: this.apiName,...config });
  

  getAllActiveEvents = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResult<EventDisplayDto>>({
      method: 'GET',
      url: '/api/events/get-active-events',
    },
    { apiName: this.apiName,...config });
  

  getEventByIdByEventId = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<EventDisplayDto>>({
      method: 'GET',
      url: `/api/events/get-event-byid/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getEventRegistrationsByEventId = (eventId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResult<EventRegistrationDisplayDto>>({
      method: 'GET',
      url: `/api/events/get-event-registrations/${eventId}`,
    },
    { apiName: this.apiName,...config });
  

  getUserEvents = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResult<EventDisplayDto>>({
      method: 'GET',
      url: '/api/events/get-user-events',
    },
    { apiName: this.apiName,...config });
  

  registerInEventByDto = (dto: EventRegistrationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<object>>({
      method: 'POST',
      url: '/api/events/register-in-event',
      body: dto,
    },
    { apiName: this.apiName,...config });
  

  updateEvent = (dto: EventUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Result<EventDisplayDto>>({
      method: 'POST',
      url: '/api/events/update-event',
      params: { id: dto.id, name: dto.name, capacity: dto.capacity, isOnline: dto.isOnline, startDate: dto.startDate, endDate: dto.endDate, link: dto.link, location: dto.location, isActive: dto.isActive },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
