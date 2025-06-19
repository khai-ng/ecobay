import { ProtectedRoute } from "@base/components";
import { OrderService } from "./orders.service";
import { useAuth } from "@base/context";
import { useEffect, useState } from "react";
import { OrderDto } from "./orders.model";
import { OrderItem } from "./order-item";
import { useNavigate } from "react-router-dom";

const Orders = () => {
    const [data, setData] = useState<OrderDto[]>([]);
    const { keycloak } = useAuth();
    const navigate = useNavigate();

    const handleItemClick = (orderId: string) => {
        navigate(`/orders/${orderId}`);
    }

    useEffect(() => {
        const fetchData = async () => {
            if (keycloak && keycloak.tokenParsed) {
                const response = await OrderService.getAsync(keycloak.tokenParsed.sub || '');
                setData(response.data || []);
            }
        };

        fetchData();
    }, [keycloak]);

    return (
        <ProtectedRoute>
            <div className="flex flex-col gap-4">
                {data.map((order, index) => <OrderItem key={index} {...order} onClick={() => handleItemClick(order.id)}/>)}
            </div>
        </ProtectedRoute>
    )
};

export default Orders;
