import type { ApplicationConfigurationDto, ApplicationConfigurationRequestOptions } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationConfigurationService {
  apiName = 'abp';
  

  get = (options: ApplicationConfigurationRequestOptions, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationConfigurationDto>({
      method: 'GET',
      url: '/api/abp/application-configuration',
      params: { includeLocalizationResources: options.includeLocalizationResources },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
