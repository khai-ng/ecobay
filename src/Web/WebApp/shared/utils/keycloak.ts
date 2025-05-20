import Keycloak from 'keycloak-js';

let keycloak: Keycloak | null = null;

export const initKeycloak = (): Keycloak => {
  if (keycloak) return keycloak;
  
  if (!process.env.NEXT_PUBLIC_KEYCLOAK_URL 
    || !process.env.NEXT_PUBLIC_KEYCLOAK_REALM 
    || !process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID) {
    throw new Error('Keycloak configuration is missing');
  }

  const keycloakConfig = {
    url: process.env.NEXT_PUBLIC_KEYCLOAK_URL,
    realm: process.env.NEXT_PUBLIC_KEYCLOAK_REALM,
    clientId: process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID
  };

  if (typeof window !== 'undefined') {
    keycloak = new Keycloak(keycloakConfig);
  }
  
  return keycloak as Keycloak;
};