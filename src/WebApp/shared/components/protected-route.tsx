import { useAuth } from './auth-context';
import React, { useEffect } from 'react';

export const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { keycloak, initialized, isAuthenticated } = useAuth();

  useEffect(() => {
    if (initialized && !isAuthenticated) {
      keycloak?.login();
    }
  }, [initialized, isAuthenticated, keycloak]);

  if (!initialized) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated) {
    return null;
  }

  return <>{children}</>;
};
