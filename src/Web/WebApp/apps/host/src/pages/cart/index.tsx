import React, { useState, useEffect } from 'react';
import { ProtectedRoute } from '@base/components/protected-route';
import CartProductItem, { CartProductItemProps } from './components/cart-item';
import { useRouter } from 'next/router';

export function Cart() {
  const [cartProducts, setCartProducts] = useState<CartProductItemProps[]>([]);
  const [totalPrice, setTotalPrice] = useState<number>(0);
  const router = useRouter();

  useEffect(() => {
    const cartData = localStorage.getItem('cart');
    const productData = cartData
      ? (JSON.parse(cartData) as CartProductItemProps[])
      : [];

    const cartProductMapping = productData.map((x) => ({
      ...x,
      check: false,
    }));
    setCartProducts(cartProductMapping);
    setTotalPrice(calculateTotalPrice(cartProductMapping));
  }, []);

  const handleProductChange = (
    check: boolean,
    index: number,
    quantity: number,
  ) => {
    const product = cartProducts[index];
    product.check = check;
    product.qty = quantity;

    setTotalPrice(calculateTotalPrice(cartProducts));
  };

  const calculateTotalPrice = (products: CartProductItemProps[]) => {
    if (!products) {
      return 0;
    }
    return products
      .filter((x) => x.check)
      .reduce((acc, product) => acc + product.price * product.qty, 0);
  };

  const handleSelectAll = (check: boolean) => {
    cartProducts.forEach((product) => {
      product.check = check;
    });
    setTotalPrice(calculateTotalPrice(cartProducts));
  };

  const handleCheckout = () => {
    localStorage.setItem(
      'checkout',
      JSON.stringify(cartProducts.filter((x) => x.check)),
    );

    router.push('/checkout');
  };

  return (
    <ProtectedRoute>
      <div className='py-5'>
        <div className="flex items-center gap-4">
          <div className="max-w-6">
            <input
              type="checkbox"
              className="w-full"
              onChange={(event) =>
                handleSelectAll(event.target.checked)
              }></input>
          </div>
          <div className="basis-[50%]">Product</div>
          <div className="flex-1 text-center">Price</div>
          <div className="flex-1 text-center">Qty</div>
          <div className="flex-1 text-center">Total</div>
          <div className="flex-1 text-center">Actions</div>
        </div>

        {cartProducts.map((product, index) => (
          <CartProductItem
            key={index}
            {...product}
            onChange={(check: boolean, quantity: number) =>
              handleProductChange(check, index, quantity)
            }
          />
        ))}

        <div className="flex items-center gap-4">
          <div className="flex-1"></div>
          <div className="flex gap-4">
            <span>Total:</span>
            <span>${totalPrice.toFixed(2)}</span>
          </div>
          <button
            className="bg-violet-800 text-white px-16 py-2 rounded-md"
            onClick={handleCheckout}>
            Checkout
          </button>
        </div>
      </div>
    </ProtectedRoute>
  );
}

export default Cart;
