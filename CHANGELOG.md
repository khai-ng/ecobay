# Changelog

All notable changes to the Ecobay project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial project setup with microservices architecture
- Product service with MongoDB integration
- Identity service with Keycloak integration
- Order management system with event sourcing
- Shopping cart functionality
- Payment processing service
- API Gateway for service orchestration
- Monitoring stack integration:
  - Grafana for visualization
  - Prometheus for metrics collection
  - Loki for log aggregation
  - Jaeger for distributed tracing
- Docker containerization for all services
- Consistent hashing implementation for distributed systems
- OpenTelemetry integration for observability
- Core libraries and shared components:
  - Core domain entities and contracts
  - Common AspNetCore configurations
  - Autofac dependency injection setup
  - MongoDB and EntityFramework integrations
  - Kafka message broker integration
  - Marten event store setup
  - MediatR for CQRS pattern

### Security
- Authentication using Keycloak
- Authorization with JWT tokens
- Secure API endpoints with Bearer token validation

### DevOps
- Docker Compose configuration for local development
- Monitoring and logging infrastructure
- Service health checks and diagnostics

## [0.1.0] - 2025-06-02
- Initial release with basic e-commerce functionality
- Microservices foundation
- Core infrastructure setup

[Unreleased]: https://github.com/yourusername/ecobay/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/yourusername/ecobay/releases/tag/v0.1.0