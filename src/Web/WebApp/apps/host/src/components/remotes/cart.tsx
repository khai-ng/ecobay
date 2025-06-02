import React from "react";
import { loadRemote } from '@module-federation/runtime';
import dynamic from "next/dynamic";
import PageLoader from "@base/components/page-loader";
import { ErrorBoundary } from "@base/components/error-boundary";
import { ProtectedRoute } from "@base/components/protected-route";

const Cart = () => {
    const CartRemote = dynamic(
        async () => {
            return loadRemote<React.ComponentType<any>>('order/cart')
            .then((module: any) => {
                if (module && module.CartComponent) {
                    return { default: module.CartComponent as React.ComponentType<any> };
                }
                throw new Error('Failed to load remote module');
            })
        },
        { ssr: true, loading: () => <PageLoader label="Loading cart..." /> }
    );

    return (
        <ErrorBoundary>
            <ProtectedRoute>
                <CartRemote />
            </ProtectedRoute>
        </ErrorBoundary>
    );
};

export default Cart;