import React from "react";
// import product from './product.module.css';
import Image from "next/image";

interface ProductProps {
    uri: string,
    alt: string,
    name: string,
    price: string,
}

const ProductItemComponent = (props: ProductProps) => {
    return (
        <div className="flex flex-col gap-3 p-3 rounded-md shadow-sm shadow-slate-400">
            <div className="m-auto max-h-52">
                <Image
                    src={props.uri}
                    alt={props.alt}
                    width={200}
                    height={200}
                    className="h-full object-contain"
                />
            </div>
            
            <span className="font-semibold line-clamp-2">{props.name}</span>
            <span>{props.price}</span>
        </div>
    )
}

export default ProductItemComponent;