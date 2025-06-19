import { AppResultFrom, httpClient } from "@base/utils";
import { EndPoints } from "../../utils/endpoints";
import { OrderDto } from "./orders.model";

export class OrderService {
    static async getAsync(buyerId: string) {
        const response = await httpClient.get(EndPoints.order, { params: { buyerId: buyerId } });
        return AppResultFrom<OrderDto[]>(response);
    }
}