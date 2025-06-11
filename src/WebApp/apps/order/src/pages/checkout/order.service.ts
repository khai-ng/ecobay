import { AppResultFrom, httpClient } from "@base/utils";
import { OrderRequest } from "./order.model";
import { EndPoints } from "../../utils/endpoints";

export class OrderService {
    static async addOrder(request: OrderRequest) {
        const response = await httpClient.post(EndPoints.order, request);
        return AppResultFrom<string>(response);
    }
}