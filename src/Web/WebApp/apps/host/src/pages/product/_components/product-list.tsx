import React from "react";
import ProductItem from "./product-item";
// import product from './product.module.css';

const products = [
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 1', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$10' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 2', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$20' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
    { uri: '/images/iphone-16-pro-max.webp', alt: 'Product 3', name: 'iPhone 16 Pro Max 256GB | Chính hãng VN/A', price: '$30' },
];

const ProductList = () => {
    return (
      <div className="app_container grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3">
        {products.map((p, i) => (
          <ProductItem
            key={i}
            uri={p.uri}
            alt={p.alt}
            name={p.name}
            price={p.price}
          />
        ))}
      </div>
    );
}

export default ProductList;