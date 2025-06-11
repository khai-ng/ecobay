import { lazy } from 'react';

const RemoteCart = lazy(
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    async () => import('order/app'),
);

export default RemoteCart;