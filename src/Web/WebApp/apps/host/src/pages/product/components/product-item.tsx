import React from "react";
import Image from "next/image";

export interface ProductProps {
    id: string,
    image: string,
    alt: string,
    name: string,
    price: number,
}

const ProductItem = (props: ProductProps) => {

    const handleAddToCart = () => {
        localStorage.setItem('cart', JSON.stringify([
            ...JSON.parse(localStorage.getItem('cart') || '[]'),
            props
        ]));
    }

    return (
        <div className="flex flex-col gap-3 p-3 rounded-md shadow-sm shadow-slate-400">
            <div className="m-auto max-h-52">
                <Image
                    src={props.image}
                    alt={props.alt}
                    width={120}
                    height={120}
                    className="h-full w-full object-contain"
                />
            </div>
            
            <span className="font-semibold line-clamp-2">{props.name}</span>
            <span>{props.price}</span>
            <button className="bg-blue-500 text-white py-1 rounded-md" onClick={handleAddToCart}>Add to cart</button>
        </div>
    )
}

export default ProductItem;