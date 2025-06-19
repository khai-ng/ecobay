import { AppResultFrom, httpClient } from "@base/utils";
import { OrderRequest } from "./checkout.model";
import { EndPoints } from "../../utils/endpoints";

export class CheckoutService {
    static async addOrderAsync(request: OrderRequest) {
        const response = await httpClient.post(EndPoints.order, request);
        return AppResultFrom<string>(response);
    }
}