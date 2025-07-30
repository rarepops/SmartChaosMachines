# SmartChaosMachines Platform - Architecture

## System Overview

The **SmartChaosMachines** platform currently represents a **distributed monolith** architecture. What we have implemented so far is primarily just a slice of the full proposed solution.

This slice focuses on the core functionalities for controlling and monitoring industrial counting machines in manufacturing environments. The platform uses **.NET Aspire** for orchestration and follows **Clean Architecture** principles, providing a foundation that will be expanded toward a more distributed microservices architecture in the future.

## Current Implementation Status

- **Architecture Pattern**: Distributed monolith with tightly coupled components deployed as a single cohesive unit
- **Scope**: Core slice of the full envisioned SCM platform solution
- **Components**: Orchestration, machine management, API exposure, and observability
- **Separation**: Internal separation of concerns but not yet fully distributed microservices

## Core Components

### Application Host (AppHost)

- **Orchestrates** the entire platform using .NET Aspire
- **Manages service discovery** and inter-service communication
- **Handles deployment** across development environments

### LineControl Service

The primary service managing counting machine operations:

- **API Layer**: REST endpoints, Swagger documentation, HTTP/HTTPS on ports 5000/5001
- **Application Layer**: Use cases (GetMachineData, ConfigureMachine, GetAllMachines), services, validation
- **Domain Layer**: Entities, interfaces, enums for machine states and operations
- **Infrastructure Layer**: Machine management, OPC-UA simulation, background services, repositories

## Technology Stack

- **.NET 10**: Core platform framework
- **.NET Aspire**: Service orchestration and observability
- **OpenTelemetry**: Distributed tracing and metrics
- **OPC-UA**: Industrial communication protocol (simulated)
- **Clean Architecture**: Separation of concerns across layers

## Key Features

### Observability

- Distributed tracing across components
- Health checks and structured logging
- Metrics collection for performance monitoring

### Machine Control

- Multi-machine management with connection pooling
- Real-time configuration updates and validation
- Automatic retry logic for failed operations

## Deployment

The platform is designed for **containerized deployment** with:

- **Development environment**: Local debugging and testing
- **Production readiness**: Scalable cloud deployment capabilities
- **Service discovery**: Automatic endpoint resolution
- **Load balancing**: Distributed request handling

## Future Evolution

This distributed monolith slice provides a **scalable foundation** for industrial machine control while maintaining clean separation of concerns. The architecture is designed to evolve toward the full microservices-based SCM platform outlined in the project documentation[3], supporting global factory rollout and ML integration capabilities.
