import { AppResultFrom, httpClient } from "@base/utils";
import { EndPoints } from "../../../utils/endpoints";


export class TrackingService {
    static async getAsync(orderId: string) {
        const response = await httpClient.get(`${EndPoints.order}/${orderId}/tracking`);
        return AppResultFrom<OrderTracking[]>(response);
    }
}

export interface OrderTracking {
    id: string;
    typeName: string;
    sequence: string;
    version: string;
    createdAt: string;
}