import React from "react";
import { loadRemote } from '@module-federation/runtime';
import dynamic from "next/dynamic";
import PageLoader from "@shared/components/page-loader";
import { ErrorBoundary } from "@shared/components/error-boundary";

const Order = () => {
    const ProfileRemote = dynamic(
        async () => {
            return loadRemote<React.ComponentType<any>>('ordering/ordering')
            .then((module: any) => {
                if (module && module.default) {
                    return { default: module.default as React.ComponentType<any> };
                }
                
                throw new Error('Failed to load remote module');
            })
        },
        { ssr: true, loading: () => <PageLoader label="Loading profile remote..." /> }
    );
	return (
        <ErrorBoundary>
            <ProfileRemote />
        </ErrorBoundary>
	);
};

export default Order;