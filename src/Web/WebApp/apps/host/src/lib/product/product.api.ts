import { AppResult, PagingResponse } from '@base/utils/api.model';
import httpClient, { AppResultFrom } from '@base/utils/axios-client';
import { endPoints } from '@app/utils/endpoints';
import { GetProductRequest, ProductItemDto } from './product.model';

export async function getProductsAsync(request: GetProductRequest): Promise<AppResult<PagingResponse<ProductItemDto>>> {
    //await new Promise(r => setTimeout(r, 3000));
    const response = await httpClient.get(endPoints.product, { params: request });
    
    return AppResultFrom(response);
}