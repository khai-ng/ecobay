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
    images: Image[] | null;
    videos: Video[] | null;
    store: string | null;
    categories: string[] | null;
    details: any | null;
}

export interface Image {
    thumb: string;
    large: string;
    variant: string;
    hires: string | null;
}


export interface Video {
    title: string;
    url: string;
    userId: string;
}
