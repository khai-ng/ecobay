import { endPoints } from '../../../utils/endpoints';
import httpClient from '@shared/utils/axios-client';
import { OrderRequest } from './order.model';
import { HttpResult } from '@shared/utils/api.model';

export async function addOrder(request: OrderRequest): Promise<HttpResult<string> | null> {
  const response = await httpClient.post(endPoints.order, request);

  if(response.status !== 200) return null;
  return response.data as HttpResult<string>;
}

