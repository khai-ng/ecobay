import React, { useState, useEffect } from 'react';
import { ProtectedRoute } from '@shared/components/protected-route';
import { useRouter } from 'next/router';
import Link from 'next/link';
import { CartProductItemProps } from '../cart/components/cart-item';
import Image from 'next/image';
import { OrderRequest } from './lib/order.model';
import { addOrder } from './lib/order.api';

export function Checkout() {
  const [checkoutItems, setCheckoutItems] = useState<CartProductItemProps[]>([]);
  
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
      localStorage.getItem('checkoutItems') || '[]',
    ) as CartProductItemProps[];

    if (items.length === 0) {
      // Redirect to cart if no items
      router.push('/cart');
    } else {
      setCheckoutItems(items);
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

    const response = await addOrder(orderRequest);
    console.log('Order response:', response);
    localStorage.removeItem('checkoutItems');
  };

  return (
    <ProtectedRoute>
      <div className="max-w-6xl mx-auto p-4">
        <h1 className="text-3xl font-bold mb-8">Checkout</h1>

        <div className="flex flex-col gap-8">
          <form
            onSubmit={handleSubmit}
            className="bg-white rounded-lg shadow p-6">
            <h2 className="text-xl font-bold mb-4">Shipping Information</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
              <div>
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
                  className="w-full p-2 border rounded">
                  <option value="">Select Country</option>
                  <option value="VN">Viet Nam</option>
                  <option value="US">United States</option>
                  <option value="CA">Canada</option>
                  <option value="UK">United Kingdom</option>
                  <option value="AU">Australia</option>
                </select>
              </div>
              <div>
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
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
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
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
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
                  className="w-full p-2 border rounded"
                />
              </div>
            </div>

            <div className="mt-6">
              <button
                type="submit"
                className="w-full bg-blue-600 text-white py-3 rounded-lg hover:bg-blue-700 font-medium">
                Complete Order
              </button>
            </div>
          </form>

          <div>
            <div className="bg-white rounded-lg shadow p-6">
              <h2 className="text-xl font-bold mb-4">Order Summary</h2>

              <div className="mb-4">
                {checkoutItems.map((item) => (
                  <div
                    key={item.id}
                    className="flex justify-between items-center py-3 border-b">
                    <div className="flex items-center">
                      <Image
                        src={item.image ?? ''}
                        alt={item.title ?? ''}
                        width={160}
                        height={160}
                        className="w-12 h-12 object-cover rounded mr-3"
                      />
                      <div>
                        <div className="font-medium">{item.title}</div>
                        <div className="text-sm text-gray-600">
                          Qty: {item.qty}
                        </div>
                      </div>
                    </div>
                    <div className="font-medium">
                      ${(item.price * item.qty).toFixed(2)}
                    </div>
                  </div>
                ))}
              </div>

              <Link
                href="/cart"
                className="block text-center text-blue-600 hover:underline">
                Return to Cart
              </Link>
            </div>
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
}

export default Checkout;
