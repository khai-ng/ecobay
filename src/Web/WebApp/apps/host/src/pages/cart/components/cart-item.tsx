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

  const quantityChangeEvent = (e: React.ChangeEvent<HTMLInputElement>) => {
    const updatedQty = parseInt(e.target.value);
    if (isNaN(parseInt(e.target.value))) return;

    setProduct({ ...product, qty: updatedQty });
    if (props.onChange) {
      props.onChange(product.check, updatedQty);
    }
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
        <div className="flex flex-col gap-1">
          <span className="line-clamp-2 text-ellipsis">{product.title}</span>
        </div>
        <div className="flex flex-col gap-1">
          <span>Variations</span>
          <span>Default</span>
        </div>
      </div>
      <div className="flex-1 text-center">₫{product.price}</div>
      <div className="flex-1 text-center">
        <input
          type="text"
          inputMode="numeric"
          pattern="[0-9]*"
          className="w-10"
          onChange={quantityChangeEvent}
          value={product.qty}
        />
      </div>
      <div className="flex-1 text-center">₫{product.qty * product.price}</div>
      <div className="flex-1 text-center">Delete</div>
    </div>
  );
};

export default CartItem;
