import React, { useEffect, useState } from "react";
import ProductItemComponent from "./product-item";
import {homepageService, } from "../../homepage.service";
import { PagingResponse } from "@shared/utils/api.model";
import { ProductItem } from "../../homepage.model";

// import product from './product.module.css';

const ProductListComponent = () => {

  const [products, setProducts] = useState<ProductItem[] | null>(null);

  useEffect(() => {
    const fetchProductData = async () => {
      const response = await homepageService.getProductListAsync();
      setProducts(response?.data ?? []);
    }

    fetchProductData();
  }, []);

  if(!products) {
    return (
      <div className="app_container">Loading</div>
    )
  }
  return (
    <div className="app_container grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3">
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