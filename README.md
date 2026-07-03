# Wayback
Wayback is a full-stack platform for Y2K and vintage-inspired fashion. Built with JS, TailwindCSS, ASP.NET Core Web API and PostgreSQL. Features include product variant management, JWT authentication, MercadoPago payment integration, an admin dashboard for order and inventory management. Deployed on Render with Docker.

For now in this repository we have only the backend API of the project:
# Wayback - Backend API

RESTful API backend for the Wayback e-commerce platform, specializing in Y2K and vintage-inspired fashion. Built with modern .NET technologies to provide robust, scalable, and secure endpoints for both public storefront consumption and administrative management.

## Tech Stack

- **Framework:** ASP.NET Core 10 Web API
- **Language:** C#
- **ORM:** Entity Framework Core 10
- **Database:** PostgreSQL
- **Authentication:** JSON Web Tokens (JWT) & BCrypt for secure password hashing
- **Deployment:** Dockerized and hosted on Render

## Core Features

### Authentication & Authorization
- Secure JWT-based authentication system.
- Role-based access control separating Admin operations from Client interactions.
- Secure password hashing using BCrypt.Net-Next.

### Product & Inventory Management
- Hierarchical product structure supporting complex variations (Products -> Variants -> Sizes/Colors).
- Real-time stock calculation and inventory tracking.
- High-performance filtering, sorting, and pagination logic executed directly at the database level.
- Dynamic catalog organization using Categories and Styles.

### Order Processing
- End-to-end order creation and status tracking workflows.
- MercadoPago payment gateway integration support.
- Secure processing of user delivery addresses and contact information.

### Admin Capabilities
- Complete CRUD operations for the entire catalog ecosystem.
- Dashboard endpoints for reviewing sales metrics, managing active orders, and adjusting inventory levels.

## Database Architecture

The PostgreSQL database schema is highly normalized and managed via Entity Framework Core (Code-First). 

👉 **[View the full Database Schema and ER Diagram here](./DATABASE.md)**

## Project Architecture

The architecture follows a clean organization pattern within the ASP.NET Core template:

- **/Controllers:** Defines the API endpoints, logically grouped into `Admin`, `Public`, `Auth`, and `ClientesVista` scopes.
- **/DTOs:** Data Transfer Objects used to shape the data sent to and received from the frontend, ensuring data privacy and preventing over-posting.
- **/Models:** Entity Framework Core entity classes mapping directly to the PostgreSQL database tables.
- **/Data:** Contains the `WaybackContext` for managing database sessions and migrations.

## Getting Started

### Prerequisites
- .NET 10 SDK
- PostgreSQL

### Local Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Cjoshue18/Wayback.git
   cd Wayback/BackEnd
   ```

2. **Configure the Environment:**
   Update `appsettings.json` or your local environment secrets with your database credentials and a secure JWT secret:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=wayback;Username=postgres;Password=yourpassword"
     },
     "JwtSettings": {
       "SecretKey": "your_super_secret_key_here",
       "Issuer": "WaybackBackEnd",
       "Audience": "WaybackFrontEnd",
       "ExpirationDays": 7
     }
   }
   ```

3. **Run Database Migrations:**
   Ensure your database is up to date with the latest schema:
   ```bash
   dotnet ef database update
   ```

4. **Run the Application:**
   ```bash
   dotnet run
   ```
   The API will start and expose endpoints. You can explore the available routes using the built-in OpenAPI/Swagger documentation if running in the Development environment.

## Docker Support

This application is fully containerized. A `Dockerfile` is included at the root of the backend directory.

To build and run the image locally:
```bash
docker build -t wayback-backend .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="Your_Conn_String" wayback-backend
```
