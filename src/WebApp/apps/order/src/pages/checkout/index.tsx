import React, { useState, useEffect } from 'react';
import { ProtectedRoute } from '@base/components/protected-route';
import { useRouter } from 'next/router';
import { CartProductItemProps } from '../cart/components/cart-item';
import Image from 'next/image';
import { OrderRequest } from '../../lib/checkout/order.model';
import { addOrder } from '../../lib/checkout/order.api';
import { ProductItemProps } from '@app/components/product-item';

export function Checkout() {
  const [checkoutItems, setCheckoutItems] = useState<CartProductItemProps[]>([]);

  const [totalPrice, setTotalPrice] = useState<number>(0);

  const [formData, setFormData] = useState({
    country: '',
    city: '',
    district: '',
    street: '',
  });

  const router = useRouter();

  useEffect(() => {
    // Get cart data from localStorage
    const items = JSON.parse(
      localStorage.getItem('checkout') || '[]',
    ) as CartProductItemProps[];

    if (items.length === 0) {
      // Redirect to cart if no items
      router.push('/cart');
    } else {
      setCheckoutItems(items);
      setTotalPrice(calculateTotalPrice(items));
    }
  }, [router]);

  const handleInputChange = (e: any) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e: any) => {
    e.preventDefault();

    const orderRequest: OrderRequest = {
      buyerId: '0fc353cb-c8aa-48c0-a8e6-45abbfab7f1f',
      paymentId: 'cabbdc63-4021-4953-be3f-99b8f0590d5f',
      country: formData.country,
      city: formData.city,
      district: formData.district,
      street: formData.street,
      orderItems: checkoutItems.map((item) => ({
        productId: item.id,
        price: item.price,
        qty: item.qty,
      })),
    };

    const result = await addOrder(orderRequest);
    if(!result.isSuccess) {
      alert(`Checkout failed: ${result.message}`);
      return;
    }

    alert('Checkout successfully');
    localStorage.removeItem('checkout');
    removeCartItems();
    router.push('/');
  };

  const removeCartItems = () => {
    const cartData = localStorage.getItem('cart');
    const productData = cartData
      ? (JSON.parse(cartData) as ProductItemProps[])
      : [];

    checkoutItems.forEach(item => {
      const index = productData.findIndex((x) => x.id === item.id);
      if (index !== -1) {
        productData.splice(index, 1);
      }
    });

    localStorage.setItem('cart', JSON.stringify(productData));
  };

  const calculateTotalPrice = (products: CartProductItemProps[]) => {
    if (!products) {
      return 0;
    }
    return products.reduce(
      (acc, product) => acc + product.price * product.qty,
      0,
    );
  };

  return (
    <ProtectedRoute>
      <div className="py-5">
        <div className="flex flex-col gap-8">
          <form onSubmit={handleSubmit}>
            <div className="font-bold mb-4">Delivery Address</div>
            <div className="grid grid-cols-3 gap-4 mb-6">
            <section className='col-span-3'>
                <label
                  className="block text-sm font-medium mb-1"
                  htmlFor="street">
                  Street
                </label>
                <input
                  type="text"
                  id="street"
                  name="street"
                  value={formData.street}
                  onChange={handleInputChange}
                  required
                  className="w-full p-2 border border-gray-400 rounded"
                />
              </section>
              <section>
                <label
                  className="block text-sm font-medium mb-1"
                  htmlFor="country">
                  Country
                </label>
                <select
                  id="country"
                  name="country"
                  value={formData.country}
                  onChange={handleInputChange}
                  required
                  className="w-full p-2 border border-gray-400 rounded">
                  <option value="">Select Country</option>
                  <option value="VN">Viet Nam</option>
                  <option value="US">United States</option>
                  <option value="CA">Canada</option>
                  <option value="UK">United Kingdom</option>
                  <option value="AU">Australia</option>
                </select>
              </section>
              <section>
                <label
                  className="block text-sm font-medium mb-1"
                  htmlFor="city">
                  City
                </label>
                <input
                  type="text"
                  id="city"
                  name="city"
                  value={formData.city}
                  onChange={handleInputChange}
                  required
                  className="w-full p-2 border border-gray-400 rounded"
                />
              </section>
              <section>
                <label
                  className="block text-sm font-medium mb-1"
                  htmlFor="district">
                  District
                </label>
                <input
                  type="text"
                  id="district"
                  name="district"
                  value={formData.district}
                  onChange={handleInputChange}
                  required
                  className="w-full p-2 border border-gray-400 rounded"
                />
              </section>
              
            </div>
            <div>
              <div>
                <h2 className="font-bold mb-4">Products Ordered</h2>

                <div className="mb-4">
                  <div className="flex items-center gap-4">
                    <div className="basis-[50%]">Product</div>
                    <div className="flex-1 text-center">Price</div>
                    <div className="flex-1 text-center">Qty</div>
                    <div className="flex-1 text-center">Total</div>
                  </div>

                  {checkoutItems.map((item) => (
                    <div key={item.id} className="flex items-center gap-4">
                      <div className="basis-[50%] flex gap-3">
                        <div className="w-20 h-20 shrink-0">
                          <Image
                            src={item.image ?? ''}
                            alt={item.title ?? ''}
                            width={160}
                            height={160}
                            className="w-full h-full object-contain"
                          />
                        </div>
                        <div className="flex flex-col gap-1 flex-1">
                          <span className="line-clamp-2 text-ellipsis">
                            {item.title}
                          </span>
                        </div>
                        <div className="flex flex-col justify-center text-gray-500">
                          <span>Variations</span>
                          <span>Default</span>
                        </div>
                      </div>
                      <div className="flex-1 text-center">${item.price}</div>
                      <div className="flex-1 text-center flex justify-center">
                        <span>{item.qty}</span>
                      </div>
                      <div className="flex-1 text-center">
                        ${(item.price * item.qty).toFixed(2)}
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="grid grid-cols-[1fr_max-content_max-content] gap-4">
                <span className="col-start-2">Merchandise Subtotal:</span>
                <span className="col-start-3 justify-self-end">${totalPrice.toFixed(2)}</span>
                <span className="col-start-2">Shipping Subtotal:</span>            
                <span className="col-start-3 justify-self-end">$0</span>
                <span className="col-start-2 self-end">Total:</span>
                <span className="col-start-3 justify-self-end text-3xl font-bold">
                  ${(totalPrice + 0).toFixed(2)}
                </span>           
                <button
                  type="submit"
                  className="col-start-2 col-span-2 justify-self-end w-56 bg-violet-800 text-white px-16 py-2 rounded-md">
                  Place Order
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </ProtectedRoute>
  );
}

export default Checkout;
