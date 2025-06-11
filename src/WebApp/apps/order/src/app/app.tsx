import { Route, Routes } from 'react-router-dom';
import localRoutes from '../routes';

export const App = () => (
  <Routes>
    {localRoutes.map(route => (
      <Route
        key={route.path}
        path={route.path}
        element={route.element}
        Component={route.Component}
      />
    ))}
  </Routes>
);

export default App;