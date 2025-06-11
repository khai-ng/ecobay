import { ProductItemDto } from "./product.model";

export interface ProductItemProps extends ProductItemDto {
  discountRate?: number;
  discountPrice?: number;
  className?: string;
  onClick?: () => void;
  onAddToCart?: () => void;
};

export const ProductItem = (props: ProductItemProps) => {
  const handleAddToCart = () => {
    localStorage.setItem(
      'cart',
      JSON.stringify([
        ...JSON.parse(localStorage.getItem('cart') || '[]'),
        { ...props, qty: 1 }
      ]),
    );
  };

  return (
    <div
      className={`relative flex flex-col gap-2 p-3`}
      onClick={props.onClick}>
      {props.discountRate && (
        <div className="absolute text-xs text-white bg-red-600 top-3 left-3 py-1 px-2">
          {props.discountRate}%
        </div>
      )}
      <div className="max-w-full aspect-square">
        <img src={props.image ?? ''} alt={props.title} width={170} height={170} className="h-full w-full object-contain"></img>
      </div>
      <div className="flex flex-1 flex-col gap-1">
        <span className="font-medium text-sm line-clamp-2 h-10">
          {props.title}
        </span>
        <span className="text-xs">
          <i className="fa fa-star text-yellow-400 pr-1"></i>
          <span>{props.averageRating}</span>
        </span>
        <div>
          <span className="text-xl text-red-600 font-semibold pr-2">
            {props.discountPrice ?? props.price}
          </span>
          {props.discountPrice && (
            <span className="text-sm text-gray-400 line-through">
              {props.price}
            </span>
          )}
        </div>
        <button
          className="text-left text-primary border border-primary rounded-3xl py-1.5 px-4"
          onClick={props.onAddToCart || handleAddToCart}>
          Add to cart
        </button>
      </div>
    </div>
  );
};

export default ProductItem;
