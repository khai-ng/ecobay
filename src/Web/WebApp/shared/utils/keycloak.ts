import Keycloak from 'keycloak-js';

const keycloakConfig = {
    url: process.env.NEXT_PUBLIC_KEYCLOAK_URL || 'http://localhost:5101',
    realm: process.env.NEXT_PUBLIC_KEYCLOAK_REALM || 'ecobay',
    clientId: process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID || 'web-client'
};

let keycloak: Keycloak | null = null;

export const initKeycloak = (): Keycloak => {
  if (keycloak) return keycloak;
  
  if (typeof window !== 'undefined') {
    keycloak = new Keycloak(keycloakConfig);
  }
  
  return keycloak as Keycloak;
};