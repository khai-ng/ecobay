export interface OrderRequest {
    buyerId: string;
    paymentId: string;
    country: string;
    city: string;
    district: string;
    street: string;
    orderItems: OrderItemRequest[];
}

export interface OrderItemRequest {
    productId: string;
    price: number;
    qty: number;
}