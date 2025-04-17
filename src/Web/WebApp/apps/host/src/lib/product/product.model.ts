import {PagingRequest} from '@shared/utils/api.model';

export interface GetProductRequest extends PagingRequest {
    category: string;
}

export interface ProductItemModel {
    id: string;
    mainCategory: string;
    title: string;
    averageRating: number;
    ratingNumber: number;
    price: number | null;
    image: string | null;
    store: string | null;
}