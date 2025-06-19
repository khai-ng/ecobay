import { lazy } from 'react';
import { RouteObject } from 'react-router-dom';

const Cart = lazy(() => import('./pages/cart'));
const Checkout = lazy(() => import('./pages/checkout'));
const Orders = lazy(() => import('./pages/orders'));
const Tracking = lazy(() => import('./pages/orders/tracking'));

export const routes: RouteObject[] = [
    {
        path: '/cart',
        Component: Cart
    },
    {
        path: '/checkout',
        Component: Checkout,
    },
    {
        path: '/orders',
        Component: Orders,
    },
    {
        path: '/orders/:orderId',
        Component: Tracking,
    }
];

export default routes;