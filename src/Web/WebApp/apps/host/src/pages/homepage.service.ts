import { endPoints } from '../utils/endpoints';
import { GetProductRequest, ProductItem } from './homepage.model';
import { PagingResponse } from '@shared/utils/api.model';
import httpClient from '@shared/utils/axios-client';

export class HomepageService {

    public async getProductListAsync(request: GetProductRequest)  {
        const response = await httpClient.get(endPoints.product, {
            params: request
        })
        const rs: PagingResponse<ProductItem> = response.data.data;
        return rs;
    }
}

export const homepageService = new HomepageService();