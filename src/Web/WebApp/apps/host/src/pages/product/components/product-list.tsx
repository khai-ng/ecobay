import React, { useEffect, useState } from "react";
import ProductItem from "./product-item";
import { productService } from "../../../lib/product/product.service";
import { GetProductRequest, ProductItemModel } from "../../../lib/product/product.model";
// import product from './product.module.css';

const ProductList = (props: GetProductRequest) => {

  const [products, setProducts] = useState<ProductItemModel[] | null>(null);

  useEffect(() => {
    const fetchProductData = async () => {
      const response = await productService.getProductListAsync(props);
      setProducts(response?.data ?? []);
    }

    fetchProductData();
  }, [props]);

  if(!products) {
    return (
      <div className="">Loading</div>
    )
  }
  return (
    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3">
      {products.map((p, i) => (
        <ProductItem
          key={i}
          id={p.id}
          image={p.image ?? ""}
          alt={""}
          name={p.title}
          price={p.price ?? 0}
        />
      ))}
    </div>
  );
}

export default ProductList;