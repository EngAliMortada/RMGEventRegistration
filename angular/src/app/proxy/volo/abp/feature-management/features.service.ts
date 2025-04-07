import type { GetFeatureListResultDto, UpdateFeaturesDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FeaturesService {
  apiName = 'AbpFeatureManagement';
  

  delete = (providerName: string, providerKey: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/feature-management/features',
      params: { providerName, providerKey },
    },
    { apiName: this.apiName,...config });
  

  get = (providerName: string, providerKey: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetFeatureListResultDto>({
      method: 'GET',
      url: '/api/feature-management/features',
      params: { providerName, providerKey },
    },
    { apiName: this.apiName,...config });
  

  update = (providerName: string, providerKey: string, input: UpdateFeaturesDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/feature-management/features',
      params: { providerName, providerKey },
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
