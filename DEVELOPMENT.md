# Development Guide - BidMoto

This document contains commonly used CLI commands for developing the BidMoto project.

## 📋 Table of Contents
- [Project Setup](#project-setup)
- [Solution Management](#solution-management)
- [Project Management](#project-management)
- [Entity Framework Core](#entity-framework-core)
- [Docker Commands](#docker-commands)
- [Package Management](#package-management)
- [Useful DotNet CLI Commands](#useful-dotnet-cli-commands)

---

## Project Setup

### Initial Project Creation

```bash
# Check .NET SDK information
dotnet --info

# List available project templates
dotnet new list

# Create a new solution (creates solution based on current folder name)
dotnet new sln

# Create a new Web API project
dotnet new webapi -o src/AuctionService

# Create a new Web API project with controllers only (no minimal API)
dotnet new webapi -controllers -o src/AuctionService

# Add project to solution
dotnet sln add src/AuctionService

# Create .gitignore file
dotnet new gitignore

# Adds new class library
dotnet new classlib -o src/Contracts

# Adds reference to a class lib
dotnet add reference ../../src/Contracts 

# Add Identity Server
dotnet new install Duende.Templates
dotnet new duende-is-aspid -o src/IdentityService

# Add Gateway service

dotnet new web -o src/GatewayService

```

---

## Solution Management

```bash
# Add a project to the solution
dotnet sln add src/ProjectName/ProjectName.csproj

# Remove a project from the solution
dotnet sln remove src/ProjectName/ProjectName.csproj

# List all projects in the solution
dotnet sln list
```

---

## Project Management

```bash
# Build the project
dotnet build

# Build without restoring packages
dotnet build --no-restore

# Clean build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore

# Run the project
dotnet run

# Run with specific configuration
dotnet run --configuration Release

# Run with environment variable
dotnet run --environment Development

# Watch for changes and auto-reload (requires watch tool)
dotnet watch run

# Publish the project
dotnet publish

# Publish for specific runtime
dotnet publish -c Release -r linux-x64
```

---

## Entity Framework Core

### EF Core Tools Setup

```bash
# Install EF Core tools globally
dotnet tool install --global dotnet-ef

# Update EF Core tools
dotnet tool update --global dotnet-ef

# Verify EF Core tools installation
dotnet ef --version
```

### Database Migrations

```bash
# Create a new migration
dotnet ef migrations add "MigrationName" --project src/AuctionService

# Create migration in specific output directory
dotnet ef migrations add "Initial" --project src/AuctionService -o Data/Migrations

# Remove the last migration (if not applied to database)
dotnet ef migrations remove --project src/AuctionService

# List all migrations
dotnet ef migrations list --project src/AuctionService

# Update database to latest migration
dotnet ef database update --project src/AuctionService

# Update to specific migration
dotnet ef database update MigrationName --project src/AuctionService

# Drop the entire database
dotnet ef database drop --project src/AuctionService

# Drop database with force (no confirmation)
dotnet ef database drop --force --project src/AuctionService

# Generate SQL script from migrations
dotnet ef migrations script --project src/AuctionService

# Generate SQL script for specific migration range
dotnet ef migrations script FromMigration ToMigration --project src/AuctionService
```

### Database Context

```bash
# Scaffold DbContext from existing database
dotnet ef dbcontext scaffold "ConnectionString" Npgsql.EntityFrameworkCore.PostgreSQL --project src/AuctionService -o Models

# Get information about DbContext
dotnet ef dbcontext info --project src/AuctionService
```

---

## Docker Commands

### Docker Compose

```bash
# Start services in detached mode
docker compose up -d

# Start services and view logs
docker compose up

# Stop services
docker compose down

# Stop services and remove volumes
docker compose down -v

# View logs
docker compose logs

# View logs for specific service
docker compose logs postgres

# Follow logs
docker compose logs -f

# Rebuild and start services
docker compose up -d --build

# Stop and remove containers, networks, and volumes
docker compose down -v --remove-orphans
```

### Docker (General)

```bash
# List running containers
docker ps

# List all containers (including stopped)
docker ps -a

# View container logs
docker logs <container-id>

# Execute command in running container
docker exec -it <container-id> bash

# Stop a container
docker stop <container-id>

# Remove a container
docker rm <container-id>

# List images
docker images

# Remove an image
docker rmi <image-id>

# Prune unused resources
docker system prune
```

---

## Package Management

### Add Packages

```bash
# Add a NuGet package
dotnet add package PackageName

# Add package to specific project
dotnet add src/AuctionService package PackageName

# Add package with specific version
dotnet add package PackageName --version 1.0.0

# Add Entity Framework PostgreSQL provider
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Add AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

# Add Swagger/OpenAPI
dotnet add package Swashbuckle.AspNetCore
```

### Remove Packages

```bash
# Remove a package
dotnet remove package PackageName

# Remove package from specific project
dotnet remove src/AuctionService package PackageName
```

### List Packages

```bash
# List all packages in project
dotnet list package

# List outdated packages
dotnet list package --outdated

# List packages with vulnerabilities
dotnet list package --vulnerable
```

---

## Useful DotNet CLI Commands

### General Information

```bash
# Display .NET SDK information
dotnet --info

# Display .NET SDK version
dotnet --version

# Display help for a command
dotnet <command> --help

# Check for SDK updates
dotnet --list-sdks
```

### Testing

```bash
# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests in specific project
dotnet test tests/ProjectName.Tests

# Run tests with verbosity
dotnet test --verbosity normal
```

### Code Formatting

```bash
# Format code
dotnet format

# Format and fix code style issues
dotnet format --verify-no-changes

# Format specific project
dotnet format src/AuctionService
```

### Project Information

```bash
# Show project information
dotnet project-info

# Show project references
dotnet list reference

# Add project reference
dotnet add reference ../OtherProject/OtherProject.csproj

# Remove project reference
dotnet remove reference ../OtherProject/OtherProject.csproj
```

---

## Common Workflows

### Setting Up a New Service

```bash
# 1. Create new service project
dotnet new webapi -controllers -o src/ServiceName

# 2. Add to solution
dotnet sln add src/ServiceName

# 3. Add required packages
dotnet add src/ServiceName package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/ServiceName package AutoMapper.Extensions.Microsoft.DependencyInjection

# 4. Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# 5. Create initial migration
dotnet ef migrations add Initial --project src/ServiceName -o Data/Migrations

# 6. Update database
dotnet ef database update --project src/ServiceName
```

### Database Reset Workflow

```bash
# 1. Drop the database
dotnet ef database drop --project src/AuctionService

# 2. Remove all migrations (if needed)
# Delete migration files manually from Data/Migrations folder

# 3. Create new initial migration
dotnet ef migrations add Initial --project src/AuctionService -o Data/Migrations

# 4. Update database
dotnet ef database update --project src/AuctionService
```

### Development Workflow

```bash
# 1. Start Docker services
docker compose up -d

# 2. Restore packages
dotnet restore

# 3. Build solution
dotnet build

# 4. Run with watch (auto-reload on changes)
dotnet watch run --project src/AuctionService

# 5. In another terminal, make changes and see auto-reload
```

---

## Tips & Tricks

1. **Use `dotnet watch`** for faster development - it automatically rebuilds and restarts your application when files change.

2. **Check connection strings** before running migrations - ensure your `appsettings.json` has the correct database connection string.

3. **Use Docker Compose** for local development - it ensures consistent database setup across the team.

4. **Keep migrations small** - create migrations frequently rather than large batches of changes.

5. **Use `--no-restore`** flag when building if you've already restored packages to speed up builds.

6. **Use `--verbosity`** flag to get more detailed output when troubleshooting:
   ```bash
   dotnet build --verbosity detailed
   ```

---

## Troubleshooting

### EF Core Issues

```bash
# If migrations are out of sync, check migration status
dotnet ef migrations list --project src/AuctionService

# If database is in inconsistent state, drop and recreate
dotnet ef database drop --force --project src/AuctionService
dotnet ef database update --project src/AuctionService
```

### Package Issues

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force
```

### Build Issues

```bash
# Clean and rebuild
dotnet clean
dotnet build --no-incremental

# Clear bin and obj folders
dotnet clean --verbosity detailed
```
 docker build -f src/AuctionService/Dockerfile -t testing123 .
 docker run testing123
 docker compose build auction-svc
 docker compose up -d

---

**Note**: Replace `src/AuctionService` with the actual path to your project when using these commands. Adjust project names and paths according to your project structure.
