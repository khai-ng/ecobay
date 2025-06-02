import { ProductItemDto } from '../lib/product/product.model';

export interface ProductItemProps extends ProductItemDto {
  discountRate?: number;
  discountPrice?: number;
  className?: string;
};
