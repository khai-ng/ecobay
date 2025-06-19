import { OrderDto } from "./orders.model"

export interface OrderItemProps extends OrderDto {
    onClick?: () => void;
}

export const OrderItem = (props: OrderItemProps) => {
    return (
        <div className="border border-gray-400 rounded-md p-4" onClick={props.onClick}>
            <div className="text-right">{props.status.name}</div>
            <div className="h-[1px] my-2 bg-gray-400"></div>
            <div className="flex flex-col items-center gap-4">
                {
                    props.orderItems.map((p, idx) => (
                        <div key={idx} className="flex justify-between items-center gap-2 w-full">
                            <div className="w-24 h-24">
                                <img src={p.imageUrl} alt="img" width={160} height={160} className="w-full h-full object-contain" />
                            </div>
                            <div className="flex flex-col flex-grow gap-1">
                                <span className="line-clamp-2 text-ellipsis">{p.productName}</span>
                                <span className="text-gray-500">Variations: Default</span>
                                <span>x{p.qty}</span>
                            </div>                           
                            <span>${p.price}</span>
                        </div>
                    ))
                }
            </div>
            <div className="h-[1px] my-2 bg-gray-400"></div>
            <div className="text-right">
                <span>Total: </span>
                <span className="font-medium text-xl pl-2">${props.totalPrice}</span>
            </div>
        </div>
    )
}