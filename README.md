# ECommerce API (.NET 8 Clean Architecture)

This project is a **Clean Architecture ECommerce Web API** built with **.NET 8**.

The project demonstrates modern backend development practices including:

- Clean Architecture
- CQRS
- MediatR
- FluentValidation
- Repository Pattern
- Global Exception Middleware
- Pagination
- Docker
- SQL Server

## Architecture

This project follows **Clean Architecture principles**.

API Layer
Application Layer
Persistence Layer
Domain Layer

Request Flow:

Controller
↓
MediatR
↓
Command / Query
↓
Handler
↓
Repository
↓
Entity Framework Core
↓
SQL Server

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- MediatR
- FluentValidation
- Docker
- SQL Server
- Swagger

## Features

- Product CRUD operations
- CQRS Pattern
- MediatR Pipeline Behaviors
- FluentValidation
- Global Exception Handling
- Pagination
- Dockerized SQL Server

## Run with Docker

Run the application with Docker:

docker compose up --build

Swagger UI:

http://localhost:5000/swagger

## API Endpoints

POST /api/products

Create a product

GET /api/products?page=1&pageSize=10

Get paginated products


