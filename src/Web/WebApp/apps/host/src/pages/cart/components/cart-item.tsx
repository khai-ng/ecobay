import Image from 'next/image';
import { useEffect, useState } from 'react';
import { ProductItemProps } from '@app/components/product-item';

export interface CartProductItemProps extends ProductItemProps {
  check: boolean;
  qty: number;
  onChange?: (checked: boolean, qty: number) => void;
}

const CartItem = (props: CartProductItemProps) => {
  const [product, setProduct] = useState<CartProductItemProps>(props);

  useEffect(() => {
    setProduct({ ...props, check: props.check, qty: props.qty });
  }, [props, props.check, props.qty]);

  const handleQuantityChange = (qty: number) => {
    if (qty < 1) return;

    setProduct({ ...product, qty: qty });
    if (props.onChange) {
      props.onChange(product.check, qty);
    }
  };

  const quantityChangeEvent = (e: React.ChangeEvent<HTMLInputElement>) => {
    const updatedQty = parseInt(e.target.value);
    if (isNaN(parseInt(e.target.value))) return;

    handleQuantityChange(updatedQty);
  };

  const checkChangeEvent = (e: React.ChangeEvent<HTMLInputElement>) => {
    setProduct({ ...product, check: e.target.checked });
    if (props.onChange) {
      props.onChange(e.target.checked, product.qty);
    }
  };

  return (
    <div className="flex items-center gap-4">
      <div className="max-w-6">
        <input
          type="checkbox"
          className="w-full"
          checked={product.check}
          onChange={checkChangeEvent}
        />
      </div>
      <div className="basis-[50%] flex gap-3">
        <div className="w-20 h-20 shrink-0">
          <Image
            src={product.image ?? ''}
            alt={product.title ?? ''}
            width={160}
            height={160}
            className="w-full h-full object-contain"
          />
        </div>
        <div className="flex flex-col gap-1 flex-1">
          <span className="line-clamp-2 text-ellipsis">{product.title}</span>
        </div>
        <div className="flex flex-col justify-center text-gray-500">
          <span>Variations</span>
          <span>Default</span>
        </div>
      </div>
      <div className="flex-1 text-center">${product.price}</div>
      <div className="flex-1 text-center flex justify-center">
        <button className="px-2 border border-gray-300" onClick={() => handleQuantityChange(product.qty - 1)}>
          <i className="fa-regular fa-minus"></i>
        </button>
        <input
          type="text"
          inputMode="numeric"
          pattern="[0-9]*"
          className="border border-gray-300 px-2 py-1 w-14 text-center"
          onChange={quantityChangeEvent}
          value={product.qty}
        />
        <button className="px-2 border border-gray-300" onClick={() => handleQuantityChange(product.qty + 1)}>
          <i className="fa-regular fa-plus"></i>
        </button>
      </div>
      <div className="flex-1 text-center">${(product.price * product.qty).toFixed(2)}</div>
      <div className="flex-1 text-center">Delete</div>
    </div>
  );
};

export default CartItem;
