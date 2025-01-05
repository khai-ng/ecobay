import React from 'react';
import Banner from '../components/banner/banner';
import ProductList from './product/_components/product-list';
import { GetProductRequest } from './homepage.model';

export function Index() {
  /*
   * Replace the elements below with your own.
   *
   * Note: The corresponding styles are in the ./index.tailwind file.
   */
  const request: GetProductRequest = {
    category: 'AMAZON FASHION',
    pageIndex: 1,
    pageSize: 50
  };

  return (
    <>
      <Banner />
      <br/>
      <ProductList {...request}/>
    </>
  );
}

export default Index;
