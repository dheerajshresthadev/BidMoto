# BidMoto - Car Auction Platform

## 🚗 Overview

**BidMoto** is a microservices-based car auction platform that enables users to buy and sell vehicles through an online auction system. This project is built using .NET for backend services and Next.js for the frontend client application.

## 🏗️ Architecture

BidMoto follows a **microservices architecture** pattern, where each service is responsible for a specific business domain. This approach provides:

- **Scalability**: Each service can be scaled independently
- **Maintainability**: Services can be developed and deployed independently
- **Technology Flexibility**: Different services can use different technologies if needed
- **Fault Isolation**: Failures in one service don't cascade to others

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Client Application                        │
│                    (Next.js Frontend)                        │
└────────────────────────┬────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                    API Gateway Service                       │
│              (Routes requests to services)                   │
└─────┬──────┬──────┬──────┬──────┬──────┬──────┬─────────────┘
      │      │      │      │      │      │      │
      ▼      ▼      ▼      ▼      ▼      ▼      ▼
   ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐ ┌────┐
   │Auth│ │Auct│ │Bid │ │Srch│ │Notf│ │... │ │... │
   └────┘ └────┘ └────┘ └────┘ └────┘ └────┘ └────┘
```

## 🔧 Services

### 1. **Identity Service** (Planned)
- **Purpose**: User authentication and authorization
- **Responsibilities**:
  - User registration and login
  - JWT token generation and validation
  - User profile management
  - Role-based access control

### 2. **Gateway Service** (Planned)
- **Purpose**: API Gateway for routing and orchestration
- **Responsibilities**:
  - Request routing to appropriate microservices
  - Load balancing
  - Request aggregation
  - Cross-cutting concerns (logging, rate limiting)

### 3. **Auction Service** ✅ (Current Service)
- **Purpose**: Core auction management
- **Responsibilities**:
  - Create, read, update, and delete auctions
  - Manage auction lifecycle (Live, Finished, ReserveNotMet)
  - Store auction details (reserve price, seller, winner, etc.)
  - Manage car/item information (make, model, year, color, mileage)
- **Database**: PostgreSQL
- **Technology**: .NET 10.0, Entity Framework Core

#### Current Features:
- ✅ CRUD operations for auctions
- ✅ Auction status management
- ✅ Car/item information management
- ✅ Database migrations with EF Core
- ✅ AutoMapper for DTO mapping
- ✅ RESTful API endpoints

#### API Endpoints:
- `GET /api/auctions` - Get all auctions (with optional date filter)
- `GET /api/auctions/{id}` - Get auction by ID
- `POST /api/auctions` - Create new auction
- `PUT /api/auctions/{id}` - Update auction
- `DELETE /api/auctions/{id}` - Delete auction

### 4. **Bid Service** (Planned)
- **Purpose**: Handle bidding operations
- **Responsibilities**:
  - Process bid submissions
  - Validate bid amounts
  - Track bid history
  - Update auction current high bid
  - Notify Auction Service of new bids

### 5. **Search Service** (Planned)
- **Purpose**: Advanced search and filtering
- **Responsibilities**:
  - Full-text search across auctions
  - Filter by car make, model, year, etc.
  - Search indexing and optimization
  - Search result ranking

### 6. **Notification Service** (Planned)
- **Purpose**: Real-time notifications
- **Responsibilities**:
  - Send notifications for new bids
  - Auction end notifications
  - Outbid notifications
  - Email/SMS notifications

### 7. **Client Application** (Planned)
- **Purpose**: Frontend user interface
- **Technology**: Next.js
- **Responsibilities**:
  - User interface for browsing auctions
  - Bid submission interface
  - User authentication UI
  - Real-time updates

## 📁 Project Structure

```
BidMoto/
├── src/
│   └── AuctionService/          # Current service implementation
│       ├── Controllers/         # API controllers
│       ├── Data/               # Database context and migrations
│       ├── DTOs/               # Data transfer objects
│       ├── Entities/           # Domain entities
│       ├── Helpers/            # Mapping profiles and utilities
│       └── Program.cs          # Application entry point
├── docker-compose.yml          # Docker services configuration
└── README.md                   # This file
```

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK
- Docker and Docker Compose
- PostgreSQL (or use Docker)

### Running the Auction Service

1. **Start PostgreSQL using Docker Compose:**
   ```bash
   docker-compose up -d
   ```

2. **Configure the connection string** in `appsettings.json` or `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=auctiondb;Username=postgres;Password=postgrespw"
     }
   }
   ```

3. **Run database migrations:**
   ```bash
   cd src/AuctionService
   dotnet ef database update
   ```

4. **Run the service:**
   ```bash
   dotnet run
   ```

5. **Access the API:**
   - Base URL: `http://localhost:5000` (or check `launchSettings.json` for the configured port)
   - Swagger/API endpoints: `http://localhost:5000/api/auctions`

> 📖 **For more detailed development commands and workflows, see [DEVELOPMENT.md](DEVELOPMENT.md)**

## 🗄️ Database Schema

### Auction Entity
- `Id` (Guid) - Primary key
- `ReservePrice` (decimal) - Minimum acceptable price
- `Seller` (string) - Seller username/identifier
- `Winner` (string) - Winner username/identifier (nullable)
- `SoldAmount` (decimal) - Final sale price (nullable)
- `CurrentHighBid` (decimal) - Current highest bid (nullable)
- `CreatedAt` (DateTime) - Auction creation timestamp
- `UpdatedAt` (DateTime) - Last update timestamp
- `AuctionEnd` (DateTime) - Auction end date/time
- `Status` (Status enum) - Auction status (Live, Finished, ReserveNotMet)

### Item Entity
- `Id` (Guid) - Primary key
- `Make` (string) - Car manufacturer
- `Model` (string) - Car model
- `Year` (int) - Manufacturing year
- `Color` (string) - Car color
- `Mileage` (int) - Car mileage
- `ImageUrl` (string) - Car image URL
- `AuctionId` (Guid) - Foreign key to Auction

## 🔄 Service Communication

As the system evolves, services will communicate through:

1. **Synchronous Communication**: HTTP/REST for request-response patterns
2. **Asynchronous Communication**: Message queues (e.g., RabbitMQ, Azure Service Bus) for event-driven patterns
3. **Service Discovery**: For dynamic service location
4. **API Gateway**: Centralized entry point for all client requests

## 🧪 Testing

(To be implemented)
- Unit tests for business logic
- Integration tests for API endpoints
- End-to-end tests for service interactions

## 📝 Development Roadmap

### Phase 1: Foundation ✅
- [x] Auction Service basic CRUD
- [x] Database setup and migrations
- [x] Basic API endpoints

### Phase 2: Core Services (In Progress)
- [ ] Identity Service implementation
- [ ] Gateway Service setup
- [ ] Bid Service implementation
- [ ] Service-to-service communication

### Phase 3: Enhanced Features
- [ ] Search Service with indexing
- [ ] Notification Service
- [ ] Real-time updates (SignalR/WebSockets)
- [ ] Caching layer (Redis)

### Phase 4: Frontend
- [ ] Next.js client application
- [ ] User authentication UI
- [ ] Auction browsing interface
- [ ] Bid submission interface

### Phase 5: Production Readiness
- [ ] Containerization (Docker)
- [ ] CI/CD pipeline
- [ ] Monitoring and logging
- [ ] Performance optimization
- [ ] Security hardening

## 🤝 Contributing

This is an initial service implementation. When contributing:

1. Follow the existing code structure and patterns
2. Ensure all new features include appropriate error handling
3. Update this README as new services are added
4. Write tests for new functionality
5. Follow .NET coding conventions

## 📚 Technology Stack

### Current Stack
- **.NET 10.0** - Backend framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **AutoMapper** - Object mapping
- **Docker** - Containerization

### Planned Stack
- **Next.js** - Frontend framework
- **Message Queue** - For async communication
- **Redis** - Caching
- **Ocelot/YARP** - API Gateway
- **Identity Server** - Authentication

## 📄 License

(To be determined)

## 👥 Team

This project is part of a larger microservices learning and development initiative.

---

**Note**: This README will be updated as new services are implemented and the architecture evolves. For detailed specifications of each service, refer to the specification documents in the project repository.
