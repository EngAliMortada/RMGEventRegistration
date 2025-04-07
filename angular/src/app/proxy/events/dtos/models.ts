import type { EntityDto } from '@abp/ng.core';

export interface EventCreationDto {
  name: string;
  capacity: number;
  isOnline: boolean;
  startDate?: string;
  endDate?: string;
  link?: string;
  location?: string;
  isActive: boolean;
}

export interface EventDeletionDto extends EntityDto<string> {
}

export interface EventDisplayDto extends EntityDto<string> {
  name?: string;
  capacity: number;
  isOnline: boolean;
  startDate?: string;
  endDate?: string;
  link?: string;
  location?: string;
  isActive: boolean;
  userCanUpdateEvent: boolean;
  userCanDeleteEvent: boolean;
  userCanCancelEvent: boolean;
  userCanRegisterInEvent: boolean;
  userCanViewRegisteredUsers: boolean;
}

export interface EventRegistrationCancelDto {
  eventId?: string;
}

export interface EventRegistrationDisplayDto {
  userName?: string;
  name?: string;
  userEmail?: string;
  registrationDate?: string;
}

export interface EventRegistrationDto extends EntityDto<string> {
}

export interface EventUpdateDto extends EventCreationDto {
  id?: string;
}
