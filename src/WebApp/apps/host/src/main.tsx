import { AuthProvider } from '@base/context';
import { Suspense } from 'react';
import * as ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './app/app';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <BrowserRouter>
    <AuthProvider>
      <Suspense fallback={<div>Loading...</div>}>
        <App />
      </Suspense>
    </AuthProvider>
  </BrowserRouter>
);
