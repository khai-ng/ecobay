import { createContext } from 'react';
import type Keycloak from 'keycloak-js';

export interface AuthContextType {
  keycloak: Keycloak | null;
  initialized: boolean;
  isAuthenticated: boolean;
}

export const AuthContext = createContext<AuthContextType>({
  keycloak: null,
  initialized: false,
  isAuthenticated: false,
});