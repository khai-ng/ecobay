import { lazy } from 'react';
import { RouteObject } from 'react-router-dom';

const Cart = lazy(() => import('./pages/cart'));
const Checkout = lazy(() => import('./pages/checkout'));

export const routes: RouteObject[] = [
    {
        path: '/cart',
        Component: Cart
    },
    {
        path: '/checkout',
        Component: Checkout,
    }
];

export default routes;