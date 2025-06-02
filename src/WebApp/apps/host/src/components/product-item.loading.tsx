import React from 'react';

const ProductItemLoading = () => {
  return (
    <div className="relative flex flex-col gap-2 p-3">
      <div className="max-w-full aspect-square bg-gray-200 animate-pulse rounded"></div>
      
      <div className="flex flex-1 flex-col gap-1">
        <div className="h-10">
          <div className="h-3 bg-gray-200 rounded w-3/4 mb-1 animate-pulse"></div>
          <div className="h-3 bg-gray-200 rounded w-1/2 animate-pulse"></div>
        </div>
        
        <div className="h-4 bg-gray-200 rounded w-16 animate-pulse"></div>
        
        <div className="flex gap-2 items-center">
          <div className="h-6 bg-gray-200 rounded w-24 animate-pulse"></div>
          <div className="h-4 bg-gray-200 rounded w-16 animate-pulse"></div>
        </div>
        
        <div className="h-9 bg-gray-200 rounded-3xl w-full animate-pulse mt-1"></div>
      </div>
    </div>
  );
};

export default ProductItemLoading;