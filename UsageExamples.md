# Ecobay Usage Examples

This document provides practical examples of using the Ecobay platform's various features and APIs.

## 🔑 Authentication

### User Authentication with Keycloak
```bash
# Get access token
curl -X POST http://localhost:5101/realms/ecobay/protocol/openid-connect/token \
  -d "grant_type=password" \
  -d "client_id=ecobay-web" \
  -d "username=demo@ecobay.com" \
  -d "password=demo123"
```

## 📦 Product Management

### Create a New Product
```http
POST http://localhost:5110/api/v1/products
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Eco-friendly Water Bottle",
  "description": "Sustainable stainless steel water bottle",
  "price": 29.99,
  "category": "Lifestyle",
  "tags": ["eco-friendly", "sustainable"]
}
```

### Search Products
```http
GET http://localhost:5110/api/v1/products?search=eco&category=Lifestyle&page=1&pageSize=10
Authorization: Bearer {your-token}
```

Products are managed by the `Product.API` service using MongoDB (see [System Architecture](./README.md#-system-architecture)).

## 🛒 Shopping Cart Operations

### Add Item to Cart
```http
POST http://localhost:5010/api/v1/cart/items
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "productId": "product123",
  "quantity": 2
}
```

### Complete Checkout
```http
POST http://localhost:5010/api/v1/orders
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "shippingAddress": {
    "street": "123 Eco Street",
    "city": "Green City",
    "country": "USA",
    "postalCode": "12345"
  },
  "paymentMethod": "credit_card"
}
```

## 📊 Monitoring and Metrics

### Access Monitoring Dashboards
- Grafana: http://localhost:3000 (metrics visualization)
- Prometheus: http://localhost:9090 (metrics collection)
- Jaeger UI: http://localhost:16686 (distributed tracing)

### Example Prometheus Query for API Latency
```promql
histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket[5m])) by (le, service))
```

## 🔄 Event Sourcing Examples

### View Event Stream for Order
```http
GET http://localhost:5010/api/v1/orders/{orderId}/events
Authorization: Bearer {your-token}
```

## 🐳 Docker Commands

### Start Specific Services
```bash
# Start only the product service and its dependencies
docker-compose up -d product-api-1 mongo-db

# Start monitoring stack
docker-compose up -d grafana prometheus loki jaeger
```

## 💡 Advanced Usage

### Implementing Custom Event Handlers
```csharp
public class OrderCreatedHandler : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle order creation event
        await ProcessOrder(notification.Order);
    }
}
```

For more detailed API documentation, refer to [README.md](./README.md).
For version history and updates, see [ChangeLog.md](./ChangeLog.md).