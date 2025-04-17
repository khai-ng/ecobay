import { endPoints } from '../../utils/endpoints';
import httpClient from '@shared/utils/axios-client';
import { OrderRequest } from './order.model';
import { HttpResult } from '@shared/utils/api.model';

class OrderService {
    async addOrder(request: OrderRequest) {
        try{
            const response = await httpClient.post(endPoints.order, request);

            if(response.status !== 200) return null;
            return response.data as HttpResult<string>;

        } catch (error) {
            console.error(error);
            return null;
        }
    }

}

export const orderService = new OrderService();