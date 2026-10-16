# 🛍️ ECommerce Platform - Learning Project

## Overview

**ECommerce** is a full-featured e-commerce web application built with **ASP.NET Core MVC** and **Razor Pages** on **.NET 10**. This is a comprehensive learning project that demonstrates modern web development practices, multi-layer architecture, and industry-standard patterns.

---

## 🎯 Project Objectives

This learning project demonstrates:

- **Multi-layered Architecture**: Separation of concerns across data access, business logic, and presentation layers
- **ASP.NET Core MVC & Razor Pages**: Building modern web applications with Microsoft's latest framework
- **Entity Framework Core**: Object-relational mapping and database design
- **Identity & Authentication**: User management, role-based access control (RBAC), and security
- **Repository Pattern**: Data access abstraction for maintainability
- **Dependency Injection**: Loose coupling and testability
- **RESTful API Design**: Proper HTTP conventions and routing

---

## 📊 Project Architecture

### Solution Structure

```
ECommerce.sln
├── ECommerce                    (Main Web Application)
│   ├── Areas/
│   │   ├── Admin/              (Administrator Dashboard)
│   │   ├── Customer/           (Customer Portal)
│   │   └── Identity/           (Authentication Pages)
│   ├── Views/
│   ├── Controllers/
│   ├── wwwroot/                (Static Assets)
│   └── Program.cs              (Application Entry Point)
│
├── ECommerce.Models            (Domain Models & ViewModels)
│   ├── Entities/
│   │   ├── Product
│   │   ├── Category
│   │   ├── Order
│   │   ├── Cart
│   │   ├── Review
│   │   ├── ApplicationUser
│   │   └── ...
│   └── ViewModels/
│       ├── ProductVM
│       ├── CartVM
│       ├── OrderVM
│       └── ...
│
├── ECommerce.DataAccess       (Data Layer)
│   ├── ApplicationDbContext    (EF Core Context)
│   └── Repository/
│       └── UnitOfWork Pattern
│
├── ECommerce.Services         (Business Logic)
│   ├── UserService
│   ├── PaymentService
│   ├── RoleService
│   └── Dependency Interfaces
│
└── ECommerce.Utility          (Shared Utilities)
```

---

## 🏗️ Technology Stack

| Layer | Technology | Purpose |
|-------|-----------|---------|
| **Frontend** | HTML5, CSS3, Bootstrap 5 | Responsive UI Design |
| **Framework** | ASP.NET Core MVC / Razor Pages | Web Application Framework |
| **.NET Version** | .NET 10 | Latest .NET Runtime |
| **Database** | SQL Server | Data Persistence |
| **ORM** | Entity Framework Core | Database Abstraction |
| **Authentication** | ASP.NET Core Identity | User Management & Security |
| **Client-side** | jQuery, Bootstrap JS | Interactive UX |

---

## 🎮 Core Features

### 👥 User Management

- **User Registration & Login** with email confirmation
- **Two-Factor Authentication (2FA)** for enhanced security
- **Password Reset** functionality
- **User Profile Management**
- **Account Recovery Codes**

### 🔐 Role-Based Access Control (RBAC)

- **Admin Dashboard** - System administration
- **Admin Roles** - Full system access
- **Customer Roles** - Limited access to shopping features
- **Role Management Interface** - Create, edit, delete roles
- **Authorization Checks** - Attribute-based access control

### 📦 Product Management

- **Product Listing** with filtering and search
- **Product Details** with images and reviews
- **Inventory Management** - Track stock levels
- **Category Management** - Organize products
- **Best-Selling Products** - Analytics dashboard

### 🛒 Shopping Features

- **Shopping Cart** with add/remove/update items
- **Wishlist** - Save favorite products for later
- **Product Reviews & Ratings** - Customer feedback
- **Checkout Process** - Streamlined ordering
- **Order Management** - Track order history

### 📈 Analytics & Admin Features

- **Dashboard** - Sales metrics and insights
- **Order Management** - Admin view of all orders
- **Low Stock Alerts** - Inventory warnings
- **User Management** - Admin user controls
- **Audit Logging** - Track system changes

### 💳 Payment Processing

- **Payment Gateway Integration** - Multiple payment methods
- **Order Status Tracking** - Pending, Processing, Shipped, Delivered
- **Payment Method Support**

---

## 🗂️ Domain Models

### Key Entities

#### **Product**
```
- ProductId (PK)
- Name
- Description
- Price
- StockQuantity
- ImageUrl
- CategoryId (FK)
- CreatedAt
```

#### **Category**
```
- CategoryId (PK)
- Name
- Description
```

#### **Order**
```
- OrderId (PK)
- UserId (FK)
- OrderDate
- TotalAmount
- OrderStatus
- ShippingAddress
- PaymentMethod
```

#### **Cart / CartItem**
```
- CartId (PK) / CartItemId (PK)
- UserId (FK)
- ProductId (FK)
- Quantity
```

#### **ApplicationUser**
- Extends ASP.NET Core Identity User
- Custom user properties
- Email, Phone, Address fields

---

## 🔄 Areas & Controllers

### Admin Area
| Controller | Responsibilities |
|------------|-----------------|
| **CategoryController** | CRUD operations for categories |
| **ProductController** | Product management & inventory |
| **UserController** | User administration |
| **RoleController** | Role management |
| **OrderController** | View and manage all orders |
| **DashboardController** | Analytics & metrics |

### Customer Area
| Controller | Responsibilities |
|------------|-----------------|
| **HomeController** | Landing page, product list |
| **ProductController** | Product details, reviews |
| **CartController** | Shopping cart, checkout |
| **OrderController** | Customer order history |
| **WishlistController** | Manage wishlist |
| **ReviewController** | Create/manage reviews |

---

## 🔒 Security Features

- **ASP.NET Core Identity** - Industry-standard user management
- **Role-Based Authorization** - Fine-grained access control
- **Email Confirmation** - Verified user accounts
- **Two-Factor Authentication** - Enhanced account security
- **HTTPS Enforcement** - Secure data transmission
- **HSTS** - HTTP Strict Transport Security
- **Audit Logging** - Track user actions (AuditLog entity)
- **Anti-CSRF Protection** - Built-in anti-forgery tokens

---

## 🚀 Design Patterns Used

### 1. **Repository Pattern**
```
UnitOfWork
├── IUserRepository
├── IProductRepository
├── IOrderRepository
├── ICartRepository
└── ...
```

### 2. **Unit of Work Pattern**
- Manages multiple repositories
- Coordinates database transactions
- Ensures data consistency

### 3. **Dependency Injection**
```csharp
// Registered in Program.cs
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IRoleService, RoleService>();
```

### 4. **Service Layer**
- Encapsulates business logic
- Provides interfaces for loose coupling
- Examples: UserService, PaymentService, RoleService

### 5. **ViewModel Pattern**
- Separates presentation concerns from domain models
- Examples: ProductVM, CartVM, OrderVM, AdminDashboardVM

---

## 📚 Learning Outcomes

By studying this project, you will understand:

1. ✅ **ASP.NET Core Architecture** - How to structure modern web applications
2. ✅ **Entity Framework Core** - Database modeling and CRUD operations
3. ✅ **MVC Pattern** - Separation of concerns in web development
4. ✅ **Authentication & Authorization** - Securing applications with Identity
5. ✅ **Dependency Injection** - Building loosely coupled, testable code
6. ✅ **API Design** - RESTful principles and HTTP conventions
7. ✅ **Database Design** - Relationships, migrations, and normalization
8. ✅ **UI/UX Development** - Bootstrap, responsive design, user interactions
9. ✅ **Business Logic** - Payment processing, order management, inventory
10. ✅ **Best Practices** - Code organization, error handling, logging

---

## 🔧 Getting Started

### Prerequisites
- .NET 10 SDK or later
- SQL Server (local or remote)
- Visual Studio 2026 or Visual Studio Code

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd ECommerce
   ```

2. **Configure Database Connection**
   - Update `appsettings.json` with your SQL Server connection string
   ```json
   {
	 "ConnectionStrings": {
	   "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDB;Integrated Security=true;"
	 }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access the Application**
   - Navigate to `https://localhost:5001` (or configured port)

---

## 📊 Application Flow

### User Journey: Product Purchase

```
Register/Login (Identity)
		 ↓
Browse Products (Customer Area)
		 ↓
View Product Details
		 ↓
Add to Cart/Wishlist
		 ↓
Proceed to Checkout
		 ↓
Enter Shipping Address
		 ↓
Select Payment Method
		 ↓
Complete Payment (PaymentService)
		 ↓
Order Created (with OrderStatus)
		 ↓
View Order History
		 ↓
Leave Reviews
```

### Admin Workflow

```
Admin Login with Role Authorization
		 ↓
Access Dashboard (DashboardController)
		 ↓
Manage Products/Categories
		 ↓
Monitor Orders
		 ↓
View Analytics (Best-selling, Low-stock)
		 ↓
Manage Users & Roles
		 ↓
Audit Log Review
```

---

## 🎓 Key Concepts Explained

### What is a Repository?
The Repository Pattern abstracts data access logic, allowing your business logic to work with data without knowing HOW it's retrieved.

### What is Unit of Work?
It manages multiple repositories and ensures that related database operations (across multiple repositories) either all succeed or all fail together.

### What is Dependency Injection?
It allows classes to receive their dependencies (services, repositories) through constructors rather than creating them internally. This improves testability and flexibility.

### What is a ViewModel?
A ViewModel shapes data for the View layer. It combines multiple domain models or provides calculated properties without exposing internal domain logic.

---

## 🧪 Code Quality Features

- **Layered Architecture** - Clean separation of concerns
- **SOLID Principles** - Maintainable and scalable code
- **Dependency Injection** - Loose coupling
- **Repository Pattern** - Data abstraction
- **Service Layer** - Business logic centralization
- **ViewModels** - Presentation concerns isolation
- **Async/Await** - Non-blocking operations
- **Error Handling** - Graceful failure handling

---

## 📋 Project Statistics

| Metric | Value |
|--------|-------|
| **Areas** | 3 (Admin, Customer, Identity) |
| **Controllers** | 13+ |
| **Models** | 25+ |
| **Entities** | 12+ |
| **Services** | 3+ |
| **Views** | 50+ |
| **Projects** | 5 (Layered Architecture) |

---

## 🔗 Areas of Extension

This learning project can be extended with:

- 📦 **Shopping Cart Optimizations** - Persistent cart storage
- 🔍 **Advanced Search** - Elasticsearch integration
- 📧 **Email Notifications** - Order status emails
- 🌐 **Multi-language Support** - Localization
- 📱 **Mobile App** - Xamarin or Flutter
- ⚡ **Caching Layer** - Redis for performance
- 🔔 **Real-time Notifications** - SignalR
- 📊 **Advanced Analytics** - BI tool integration
- 🤖 **Recommendation Engine** - ML-based suggestions
- 💬 **Customer Support Chat** - Live chat system

---

## 📖 Resources & References

### Official Documentation
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs)

### Learning Topics
- SOLID Principles
- Design Patterns (Repository, Unit of Work, Service Locator)
- RESTful API Design
- Database Normalization
- Authentication vs Authorization

---

## 💡 Key Takeaways

1. **Structure Matters** - Layered architecture makes projects maintainable
2. **Interfaces First** - Program to interfaces, not implementations
3. **Separation of Concerns** - Each layer has a single responsibility
4. **Security First** - Always implement proper authentication & authorization
5. **User Experience** - Good UX requires careful design and testing
6. **Scalability** - Design patterns enable future growth
7. **Code Reusability** - Services and repositories prevent duplication

---

## 📝 License & Usage

This is an educational project designed for learning purposes. Students can:
- Learn modern ASP.NET Core development
- Understand enterprise architecture patterns
- Practice implementing business logic
- Experiment with different technologies and features

---

## ✨ Summary

**ECommerce** serves as a comprehensive learning platform for aspiring .NET developers. It combines theoretical best practices with practical implementation, demonstrating how to build professional-grade web applications using modern technologies and design patterns.

Whether you're learning for the first time or advancing your skills, this project provides a solid foundation and reference point for enterprise web development.


