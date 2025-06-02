# Ecobay

**Ecobay** is a production-ready e-commerce platform showcasing modern distributed system design patterns. Built with a cloud-native microservices architecture, it leverages technologies like .NET 8, React, and contemporary DevOps practices. The platform demonstrates enterprise-grade patterns including event sourcing, CQRS, API gateway routing, distributed monitoring, and containerized deployment.

## 🚀 Features

- **Product Catalog Management**: Search and filter products efficiently.
- **Shopping Cart & Checkout**: Seamless cart operations and checkout workflows.
- **Order Processing**: Robust order handling with event sourcing.
- **User Authentication**: Secure user management using Keycloak.
- **Real-time Monitoring**: Comprehensive observability across services.

## 🧱 System Architecture

Ecobay follows a microservices architecture with clear separation of concerns and technology diversity based on domain requirements.

### Core Components

| Component Type | Service Name     | Technology Stack             | Port  | Purpose                             |
|----------------|------------------|------------------------------|-------|-------------------------------------|
| Frontend       | `web-client`     | Next.js 14, React            | 3001  | User interface and experience       |
| Gateway        | `web-apigateway` | YARP, ASP.NET Core 8         | 5100  | API routing and cross-cutting concerns |
| Business Logic | `product-api-1`  | .NET 8, MongoDB              | 5110  | Product catalog management          |
| Business Logic | `order-api`      | .NET 8, Entity Framework     | 5010  | Order processing and fulfillment    |
| Business Logic | `payment-api`    | .NET 8                       | TBD   | Payment processing (future)         |
| Identity       | `keycloak`       | Keycloak                     | 5101  | Authentication and authorization    |
| Messaging      | `kafka`          | Apache Kafka                 | 9092  | Asynchronous event streaming        |
| Data           | `mongo-db`       | MongoDB 7.0.9                | 27017 | Product document storage            |
| Data           | `mysql-db`       | MySQL 8.0.34                 | 3306  | Relational data and identity        |
| Data           | `pg-eventstore-db` | PostgreSQL 15              | 5432  | Event sourcing storage              |

### Shared Kernel Architecture

The platform implements a shared kernel pattern to provide consistent infrastructure across all services, facilitating code reuse and standardized practices.

## 🛠️ Technology Stack

### Backend Technologies

- **Runtime**: .NET 8 with ASP.NET Core
- **API Framework**: FastEndpoints for minimal APIs
- **Architecture Patterns**: CQRS with MediatR, Event Sourcing with Marten
- **Authentication**: Keycloak with JWT Bearer tokens
- **Data Access**: Entity Framework Core, MongoDB Driver, Marten
- **Messaging**: Apache Kafka with Confluent .NET client
- **Observability**: OpenTelemetry, Serilog structured logging
  
### Frontend Technologies
- **Framework**: Next.js 14 with React 18
- **Language**: TypeScript
- **Authentication**: NextAuth.js with Keycloak provider
- **Deployment**: Docker containerization
  
### Infrastructure Technologies
- **Containerization**: Docker with Docker Compose orchestration
- **Service Discovery**: Container networking with health checks
- **Monitoring**: Prometheus metrics, Grafana dashboards, Loki logging, Jaeger tracing
- **Development**: Hot reload, volume mounting, profile-based deployment

## 📦 Deployment

Ecobay utilizes containerized deployment with Docker Compose.

To get started:

```bash
git clone https://github.com/khai-ng/ecobay.git
cd ecobay
docker-compose up --build
```

## 📚 Documentation

For a comprehensive understanding of the system, refer to the [![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/khai-ng/ecobay)
