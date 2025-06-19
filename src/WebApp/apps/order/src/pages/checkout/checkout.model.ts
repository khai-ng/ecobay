export interface OrderRequest {
    buyerId: string;
    paymentId: string;
    country: string;
    city: string;
    district: string;
    street: string;
    orderItems: OrderItem[];
}

export interface OrderItem {
    productId: string;
    productName: string;
    imageUrl: string;
    price: number;
    qty: number;
}