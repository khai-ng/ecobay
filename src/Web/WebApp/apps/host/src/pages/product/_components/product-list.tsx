import React, { useEffect, useState } from "react";
import ProductItemComponent from "./product-item";
import {homepageService, } from "../../homepage.service";
import { GetProductRequest, ProductItem } from "../../homepage.model";
// import product from './product.module.css';

const ProductListComponent = (props: GetProductRequest) => {

  const [products, setProducts] = useState<ProductItem[] | null>(null);

  useEffect(() => {
    const fetchProductData = async () => {
      const response = await homepageService.getProductListAsync(props);
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
        <ProductItemComponent
          key={i}
          uri={p.images != null ? p.images[0].large : ""}
          alt={""}
          name={p.title}
          price={p.price ?? ""}
        />
      ))}
    </div>
  );
}

export default ProductListComponent;