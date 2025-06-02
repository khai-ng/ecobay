export interface ProductItemDto {
    id: string;
    mainCategory: string;
    title: string;
    averageRating: number;
    ratingNumber: number;
    price: number;
    image: string | null;
    store: string | null;
}