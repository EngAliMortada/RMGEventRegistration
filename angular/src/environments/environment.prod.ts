import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'EventRegistration',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44315/',
    redirectUri: baseUrl,
    clientId: 'EventRegistration_App',
    responseType: 'code',
    scope: 'offline_access EventRegistration',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44315',
      rootNamespace: 'RMG.EventRegistration',
    },
  },
} as Environment;
