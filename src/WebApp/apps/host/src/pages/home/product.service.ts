import { ProductItemDto } from "@base/components";
import { AppResult, AppResultFrom, httpClient, PagingRequest, PagingResponse } from "@base/utils";
import { EndPoints } from "../../utils/endpoints";

export interface GetProductRequest extends PagingRequest {
    category?: string;
}

export class ProductService {
    static async getProductsAsync(request: GetProductRequest): Promise<AppResult<PagingResponse<ProductItemDto>>> {
        const response = await httpClient.get(EndPoints.product, { params: request });   
        return AppResultFrom(response);
    }
}