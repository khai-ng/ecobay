import { OrderItem } from "../checkout/checkout.model";

export interface OrderDto {
    id: string;
    buyerId: string;
    paymentId: string;
    status: OrderStatus;
    totalPrice: number;
    createdAtTicks: number;
    orderItems: OrderItem[];
}

export interface OrderStatus {
    id: string;
    name: string;
}