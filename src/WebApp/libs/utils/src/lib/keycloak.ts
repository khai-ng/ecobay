import Keycloak from 'keycloak-js';

let keycloak: Keycloak | null = null;

export const initClient = (): Keycloak => {
  if (keycloak) return keycloak;
  
  if (!import.meta.env.VITE_PUBLIC_KEYCLOAK_URL 
    || !import.meta.env.VITE_PUBLIC_KEYCLOAK_REALM 
    || !import.meta.env.VITE_PUBLIC_KEYCLOAK_CLIENT_ID) {
    throw new Error('Keycloak configuration is missing');
  }

  const keycloakConfig = {
    url: import.meta.env.VITE_PUBLIC_KEYCLOAK_URL,
    realm: import.meta.env.VITE_PUBLIC_KEYCLOAK_REALM,
    clientId: import.meta.env.VITE_PUBLIC_KEYCLOAK_CLIENT_ID
  };

  if (typeof window !== 'undefined') {
    keycloak = new Keycloak(keycloakConfig);
  }
  
  return keycloak as Keycloak;
};