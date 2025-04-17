import { endPoints } from '../../utils/endpoints';
import { GetProductRequest, ProductItemModel } from './product.model';
import { PagingResponse } from '@shared/utils/api.model';
import httpClient from '@shared/utils/axios-client';

class ProductService {

    public async getProductListAsync(request: GetProductRequest)  {
        try {
            const response = await httpClient.get(endPoints.product, {
                params: request
            });
    
            if(response.status !== 200) return null;
            
            const rs: PagingResponse<ProductItemModel> = response.data.data;
            return rs;
        } catch (error) {
            console.error(error);
            return null;
        }
    }
}

export const productService = new ProductService();