import { useEffect, useState } from 'react';
import { getProductsAsync } from "../pages/lib/product.api";
import { GetProductRequest } from "../pages/lib/product.model";
import ProductItem, { ProductItemProps } from "./product-item";
import ProductItemLoading from "./product-item.loading";

export interface ProductListProps {
    className?: string;
    request: GetProductRequest;
}

const ProductListLoader = (props: ProductListProps) => {

  const [products, setProducts] = useState<ProductItemProps[]>([]);

  useEffect(() => {
      const fetchProductData = async () => {
        const response = await getProductsAsync(props.request);
        setProducts(response?.data || []);
      };
  
      fetchProductData();
  }, [props.request]);

  if(products.length == 0) {
    return (
      <div className={`${props.className}`}>
        {Array(props.request.pageSize).fill(0).map((_, index) => <ProductItemLoading key={index} />)}
      </div>
    )
  }

  return (
    <div className={`${props.className}`}>
        { products.map((product, index) =>  <ProductItem key={index} {...product}/>) }
    </div>
  );
}

export default ProductListLoader;