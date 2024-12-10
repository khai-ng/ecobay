import React from 'react';
import Banner from '../components/banner/banner';
import ProductList from './product/components/product-list';

export function Index() {
  /*
   * Replace the elements below with your own.
   *
   * Note: The corresponding styles are in the ./index.tailwind file.
   */
  return (
    <>
      <Banner />
      <br/>
      <ProductList />
    </>
  );
}

export default Index;
