import React, { createContext, useContext, useEffect, useState } from 'react';
import { initKeycloak } from '../utils/keycloak';
import type Keycloak from 'keycloak-js';

interface AuthContextType {
  keycloak: Keycloak | null;
  initialized: boolean;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType>({
  keycloak: null,
  initialized: false,
  isAuthenticated: false,
});

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [keycloak, setKeycloak] = useState<Keycloak | null>(null);
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    const kc = initKeycloak();
    let intervalId: NodeJS.Timeout;


    kc.init({
      // flow: 'implicit',

      onLoad: 'check-sso',
      silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html',
      checkLoginIframe: false, // Disable iframe checking
      pkceMethod: 'S256', // Enable PKCE
      token: localStorage.getItem('token') || undefined,
      refreshToken: localStorage.getItem('refreshToken') || undefined,
    })
    .then((authenticated) => {
      setKeycloak(kc);
      setInitialized(true);

      if(!authenticated) return;

      localStorage.setItem('token', kc.token ?? '');
      localStorage.setItem('refreshToken', kc.refreshToken ?? '');

      intervalId = setInterval(() => {
        kc.updateToken(70)
          .then((refreshed) => {
            if (refreshed) {
              localStorage.setItem('token', kc.token as string);
              localStorage.setItem('refreshToken', kc.refreshToken as string);
            }
          })
          .catch(() => {
            console.error('Failed to refresh token');
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');
            
            kc.logout();
          });
      }, 60000); // Check token every minute
    })
    .catch((error) => {
      console.error('Keycloak init error:', error);
    });

    kc.onTokenExpired = () => {
      kc.updateToken(70)
        .then((refreshed) => {
          if (refreshed) {
            console.log('Expiration token refreshed');
            localStorage.setItem('token', kc.token as string);
            localStorage.setItem('refreshToken', kc.refreshToken as string);
          }
        })
        .catch(() => {
          console.error('Failed to refresh token on expiration');
          localStorage.removeItem('token');
          localStorage.removeItem('refreshToken');
          
          kc.logout();
        });
    };

    return () => {
      clearInterval(intervalId);
    };
    
  }, []);

  return (
    <AuthContext.Provider
      value={{
        keycloak,
        initialized,
        isAuthenticated: !!keycloak?.authenticated,// && !isLocallyLoggedOut,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);