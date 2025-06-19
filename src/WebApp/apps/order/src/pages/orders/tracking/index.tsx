import { useEffect, useState } from "react";
import { OrderTracking, TrackingService } from "./tracking.service";
import { useParams } from "react-router-dom";
import { format } from "date-fns";

const Tracking = () => {
    const { orderId } = useParams<{ orderId: string }>();
    const [data, setData] = useState<OrderTracking[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            if (!orderId) return;

            const result = await TrackingService.getAsync(orderId);
            if (result.isSuccess) {
                setData(result.data || []);
            }
        };

        fetchData();
    }, [orderId]);

    return (
        <div className="flex justify-center gap-4">
            {
                data.map((o, idx) => (
                    <div key={idx} className="flex flex-col items-center gap-2">
                        <span>{idx + 1}</span>
                        <span>{o.typeName}</span>
                        <span className="text-gray-500">{ format(new Date(o.createdAt), 'dd-MM-yyyy HH:mm') }</span>
                    </div>
                ))
            }
        </div>
    );
}

export default Tracking;