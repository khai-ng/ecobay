import { RouteObject } from 'react-router-dom';
import { lazy } from 'react';

const Home = lazy(() => import('./pages/home'));
export const routes: RouteObject[] = [
    {
        path: '/',
        Component: Home,
    }
]

export default routes;