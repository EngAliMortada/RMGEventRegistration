import type { AbpLoginResult, UserLoginInfo } from './models/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiName = 'AbpAccount';
  

  checkPasswordByLogin = (login: UserLoginInfo, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AbpLoginResult>({
      method: 'POST',
      url: '/api/account/check-password',
      body: login,
    },
    { apiName: this.apiName,...config });
  

  loginByLogin = (login: UserLoginInfo, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AbpLoginResult>({
      method: 'POST',
      url: '/api/account/login',
      body: login,
    },
    { apiName: this.apiName,...config });
  

  logout = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: '/api/account/logout',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
