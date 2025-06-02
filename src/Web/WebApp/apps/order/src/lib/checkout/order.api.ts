import { endPoints } from '@app/utils/endpoints';
import httpClient, { AppResultFrom } from '@base/utils/axios-client';
import { OrderRequest } from './order.model';

export async function addOrder(request: OrderRequest) {
  const response = await httpClient.post(endPoints.order, request);
  return AppResultFrom<string>(response);
}