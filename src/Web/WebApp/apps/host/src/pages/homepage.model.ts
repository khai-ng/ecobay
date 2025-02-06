import {PagingRequest} from '@shared/utils/api.model';

export interface GetProductRequest extends PagingRequest {
    category: string;
}

export interface ProductItem {
    id: string;
    mainCategory: string;
    title: string;
    averageRating: number;
    ratingNumber: number;
    price: string | null;
    image: string | null;
    store: string | null;
}