import '@base/fonts';
import { Route, RouteObject, Routes } from 'react-router-dom';
import localRoutes from '../routes';
import { Header } from '@base/components';

// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import remoteRoutes from 'order/routes';

const routes: RouteObject[] = [...localRoutes, ...remoteRoutes];
export const App = () => (
  <main className="app_container bg-white">
    <Header></Header>
    <Routes>
      {routes.map(route => (
        <Route
          key={route.path}
          path={route.path}
          element={route.element}
          Component={route.Component}
        />
      ))}
    </Routes>
  </main>
);

export default App;