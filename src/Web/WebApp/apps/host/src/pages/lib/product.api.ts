import { PagingResponse } from '@shared/utils/api.model';
import httpClient from '@shared/utils/axios-client';
import { endPoints } from '../../utils/endpoints';
import { GetProductRequest, ProductItemDto } from './product.model';

export async function getProductsAsync(request: GetProductRequest): Promise<PagingResponse<ProductItemDto>> {
    //await new Promise(r => setTimeout(r, 3000));
    const response = await httpClient.get(endPoints.product, { params: request });
    
    if(response.status !== 200) return {
        data: [],
        hasNext: false,
        pageIndex: 0,
        pageSize: 0
    };

    return response.data.data as PagingResponse<ProductItemDto>;
}