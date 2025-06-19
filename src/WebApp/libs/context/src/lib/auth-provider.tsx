import { useEffect, useState } from "react";
import type Keycloak from 'keycloak-js';
import { initClient } from "@base/utils";
import { AuthContext } from "./auth-context";

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [keycloak, setKeycloak] = useState<Keycloak | null>(null);
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    const kc = initClient();

    const channel = new BroadcastChannel('auth');

    function updateTokens(token?: string, refreshToken?: string) {
      localStorage.setItem('token', token ?? '');
      localStorage.setItem('refreshToken', refreshToken ?? '');
      channel.postMessage({
        type: 'TOKEN_REFRESHED',
        token: token,
        refreshToken: refreshToken,
      });
    }
    channel.onmessage = (event) => {
      if (event.data.type === 'TOKEN_REFRESHED') {
        kc.token = event.data.token;
        kc.refreshToken = event.data.refreshToken;
      }
    };

    if (kc.didInitialize) {
      setKeycloak(kc);
      setInitialized(true);
      return;
    }

    kc.onTokenExpired = async () => {
      try {
        const refreshed = await kc.updateToken(70);
        if (refreshed) {
          updateTokens(kc.token, kc.refreshToken);
        }
      } catch (error) {
        console.error('Failed to refresh token on expiration:', error);
        kc.logout();
      }
    };

    const init = async () => {
      try {
        await kc.init({
          // flow: 'implicit',
          onLoad: 'check-sso',
          silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html',
          checkLoginIframe: false, // Disable iframe checking
          pkceMethod: 'S256', // Enable PKCE
          token: localStorage.getItem('token') || undefined,
          refreshToken: localStorage.getItem('refreshToken') || undefined,
        })

        setKeycloak(kc);
        setInitialized(true);
        updateTokens(kc.token, kc.refreshToken);

      } catch (error) {
        console.error('Keycloak init error:', error);
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
      }
    }

    init();

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