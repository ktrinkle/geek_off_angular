/**
* This file contains authentication parameters. Contents of this file
* is roughly the same across other MSAL.js libraries. These parameters
* are used to initialize Angular and MSAL Angular configurations in
* in app.module.ts file.
*/

import { environment } from 'src/environments/environment';

const REST_API_SERVER = environment.api_url;

/**
* Configuration object to be passed to MSAL instance on creation.
* For a full list of MSAL.js configuration parameters, visit:
* https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/configuration.md
*/

export const protectedResources = {
  geekOffApi: {
    endpoint: REST_API_SERVER + '/api',
    scopes: ["#{SCOPE}#"]
  }
}

export const roles = {
  "Player": "player",
  "Admin": "admin",
  "Host": "host",
}
