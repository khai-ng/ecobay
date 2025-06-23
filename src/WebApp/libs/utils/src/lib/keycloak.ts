import Keycloak from 'keycloak-js';

let keycloak: Keycloak | null = null;

export const initClient = (): Keycloak => {
  if (keycloak) return keycloak;
  
  if (!process.env.VITE_PUBLIC_KEYCLOAK_URL 
    || !process.env.VITE_PUBLIC_KEYCLOAK_REALM 
    || !process.env.VITE_PUBLIC_KEYCLOAK_CLIENT_ID) {
    throw new Error('Keycloak configuration is missing');
  }

  const keycloakConfig = {
    url: process.env.VITE_PUBLIC_KEYCLOAK_URL,
    realm: process.env.VITE_PUBLIC_KEYCLOAK_REALM,
    clientId: process.env.VITE_PUBLIC_KEYCLOAK_CLIENT_ID
  };

  if (typeof window !== 'undefined') {
    keycloak = new Keycloak(keycloakConfig);
  }
  
  return keycloak as Keycloak;
};