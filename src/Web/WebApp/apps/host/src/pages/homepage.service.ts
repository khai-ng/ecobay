import { endPoints } from '../utils/endpoints';
import { GetProductRequest, ProductItem } from './homepage.model';
import { HttpResult, PagingResponse } from '@shared/utils/api.model';
import httpClient from '@shared/utils/axios-client';

const requestParams: GetProductRequest = {
    category: 'AMAZON FASHION',
    pageIndex: 1,
    pageSize: 20
};

export class HomepageService {

    public async getProductListAsync()  {
        const response = await httpClient.get(endPoints.product, {
            params: requestParams
        })
        const rs: PagingResponse<ProductItem> = response.data.data;
        return rs;
    }
}

export const homepageService = new HomepageService();