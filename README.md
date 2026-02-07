# Employee Management System - Complete Setup Guide

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Backend Setup](#backend-setup)
- [Frontend Setup](#frontend-setup)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Running Unit Tests](#running-unit-tests)
- [Default Credentials](#default-credentials)
- [API Documentation](#api-documentation)
- [Troubleshooting](#troubleshooting)
- [Architecture Overview](#architecture-overview)

---

## Overview

A comprehensive Employee Management System built with ASP.NET Core 10, Angular 18, and SQL Server. This enterprise-grade application follows Clean Architecture principles with complete CRUD operations, JWT authentication, role-based authorization, and modern UI/UX.

---

## Features

### Core Features

- ✅ Employee Management (CRUD operations)
- ✅ Department Management
- ✅ Designation Management
- ✅ Salary Tracking
- ✅ Reporting Manager Hierarchy
- ✅ JWT Authentication \& Authorization
- ✅ Role-Based Access Control (Admin, HR, User)
- ✅ Search & Pagination

### Technical Features

- ✅ Clean Architecture (Backend)
- ✅ Repository & Unit of Work Pattern
- ✅ AutoMapper for DTO mapping
- ✅ FluentValidation for input validation
- ✅ Serilog for structured logging
- ✅ Performance monitoring middleware
- ✅ Global exception handling
- ✅ EF Core Code-First with Migrations
- ✅ Material Design UI (Angular Material)
- ✅ Reactive Forms with validation
- ✅ HTTP Interceptors
- ✅ Route Guards
- ✅ Comprehensive Unit Tests (Backend & Frontend)

---

## Technology Stack

### Backend

- Framework: ASP.NET Core 10 Web API
- Database: Microsoft SQL Server
- ORM: Entity Framework Core 10
- Authentication: JWT Bearer Token
- Logging: Serilog
- Testing: xUnit, Moq, FluentAssertions
- API Documentation: Swagger/OpenAPI

### Frontend

- Framework: Angular 18
- UI Library: Angular Material 18
- State Management: RxJS
- HTTP Client: Angular HttpClient
- Forms: Reactive Forms
- Testing: Jasmine, Karma
- Notifications: ngx-toastr

---

## Prerequisites

Before you begin, ensure you have the following installed:

### Required Software

#### 1. Backend Requirements

- Visual Studio 2026 (Community/Professional/Enterprise)
- .NET 10 SDK

- SQL Server 2019 or higher (LocalDB or Full Edition)

- SQL Server Management Studio (SSMS)

#### 2. Frontend Requirements

- Node.js (v18.x or v20.x LTS)

- Angular CLI (v18)

- Visual Studio Code

#### 3. Git

## Project Structure

EmployeeManagement/
│
│ ├── EmployeeManagement.Domain/ # Domain entities, enums
│ ├── EmployeeManagement.Application/ # Business logic, DTOs, services
│ ├── EmployeeManagement.Infrastructure/ # Data access, repositories
│ ├── EmployeeManagement.API/ # Web API, controllers
│ └── EmployeeManagement.Tests/ # Unit tests
│
│ └── employee-management-ui/ # Angular application
│ ├── src/
│ │ ├── app/
│ │ │ ├── components/ # UI components
│ │ │ ├── services/ # HTTP services
│ │ │ ├── models/ # TypeScript interfaces
│ │ │ ├── guards/ # Route guards
│ │ │ └── interceptors/ # HTTP interceptors
│ │ └── environments/ # Environment configs
│ └── tests/ # Unit tests
│
└── README.md

## Backend Setup

### Step 1: Clone the Repository

# Clone the repository

git clone https://github.com/yourusername/employee-management-system.git

# Navigate to the backend directory

cd employee-management-system

### Step 2: Open Solution in Visual Studio

1. Open Visual Studio 2022
2. Click "Open a project or solution"
3. Navigate to `Backend/EmployeeManagement.sln`
4. Click "Open"

### Step 3: Restore NuGet Packages

Visual Studio will automatically restore packages. If not:

```bash
# Option 1: In Visual Studio
Right-click Solution → "Restore NuGet Packages"

# Option 2: Using CLI
dotnet restore
```

### Step 4: Configure Connection String

1. Open `EmployeeManagement.API/appsettings.json`
2. Update the connection string based on your SQL Server setup:

#### For SQL Server LocalDB (Default):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

#### For SQL Server Express:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

#### For SQL Server with credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Step 5: Verify JWT Configuration

In `appsettings.json`, ensure JWT settings are present:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "EmployeeManagementAPI",
    "Audience": "EmployeeManagementClient"
  }
}
```

⚠️ Important: Change the JWT Key in production!

### Step 6: Build the Solution

```bash
# In Visual Studio
Build → Build Solution (Ctrl+Shift+B)

# Or using CLI
dotnet build
```

Expected output:

```
Build started...
1>------ Build started: Project: EmployeeManagement.Domain ------
2>------ Build started: Project: EmployeeManagement.Application ------
3>------ Build started: Project: EmployeeManagement.Infrastructure ------
4>------ Build started: Project: EmployeeManagement.API ------
========== Build: 4 succeeded, 0 failed ==========
```

---

## 🗄️ Database Setup

### Step 1: Open Package Manager Console

In Visual Studio:

1. Go to Tools → NuGet Package Manager → Package Manager Console
2. Set Default project to: `EmployeeManagement.Infrastructure`

### Step 2: Run Migrations

```powershell
# Add migration (if not present)
Add-Migration InitialCreate

# Update database
Update-Database
```

Expected output:

```
Build started...
Build succeeded.
Applying migration '20260207_InitialCreate'.
Done.
```

### Step 3: Verify Database Creation

#### Using SSMS:

1. Open SQL Server Management Studio
2. Connect to your server (e.g., `(localdb)\mssqllocaldb`)
3. Expand Databases
4. You should see: EmployeeManagementDB
5. Expand the database → Tables to verify:
   - Departments
   - Designations
   - Employees
   - Salaries
   - Users

#### Using Visual Studio:

1. Go to View → SQL Server Object Explorer
2. Expand your SQL Server instance
3. Expand Databases → EmployeeManagementDB

### Step 4: Verify Seed Data

Run this query in SSMS or SQL Server Object Explorer:

```sql
-- Check departments
SELECT * FROM Departments;

-- Check designations
SELECT * FROM Designations;

-- Check default user
SELECT * FROM Users;
```

You should see:

- 3 Departments: IT, HR, Finance
- 4 Designations: Software Engineer, Senior Software Engineer, Tech Lead, HR Manager
- 1 User: admin (for login)

---

## 🎨 Frontend Setup

### Step 1: Navigate to Frontend Directory

```bash
# From project root
cd Frontend/employee-management-ui
```

### Step 2: Install Dependencies

```bash
npm install
```

This will install all required packages. Wait for completion (may take 2-5 minutes).

Expected output:

```
added 1234 packages in 2m
```

### Step 3: Verify Angular CLI

```bash
ng version
```

Expected output:

```
Angular CLI: 18.x.x
Node: 20.x.x
Package Manager: npm 10.x.x
```

### Step 4: Configure API Endpoint

1. Open `src/environments/environment.ts`
2. Verify the API URL matches your backend:

```typescript
export const environment = {
  production: false,
  apiUrl: "https://localhost:7001/api",
};
```

⚠️ Note: The port `7001` should match your backend API port (check `launchSettings.json`)

### Step 5: Build Frontend (Optional)

```bash
# Development build
ng build

# Production build
ng build --configuration production
```

---

## ▶️ Running the Application

### Step 1: Start Backend API

#### Option A: Using Visual Studio

1. Ensure `EmployeeManagement.API` is set as startup project (should be in bold)
2. Press F5 (Debug) or Ctrl+F5 (Without Debug)
3. Browser will open with Swagger UI: `https://localhost:7001/swagger`

#### Option B: Using Command Line

```bash
cd Backend/EmployeeManagement.API
dotnet run
```

#### Verify Backend is Running:

- Swagger UI should load at: `https://localhost:7001/swagger`
- Console should show:

```
info: Microsoft.Hosting.Lifetime
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime
      Application started.
```

### Step 2: Start Frontend Application

Open a new terminal/command prompt:

```bash
cd Frontend/employee-management-ui
ng serve
```

Or with specific options:

```bash
ng serve --open --port 4200
```

#### Verify Frontend is Running:

- Console should show:

```
 Angular Live Development Server is listening on localhost:4200
✔ Compiled successfully.
```

- Browser automatically opens: `http://localhost:4200`

### Step 3: Access the Application

1. Frontend URL: http://localhost:4200
2. Backend API: https://localhost:7001
3. Swagger Documentation: https://localhost:7001/swagger

---

## 🔐 Default Credentials

### Login to the Application:

URL: http://localhost:4200/login

| Role  | Username | Password  |
| :---- | :------- | :-------- |
| Admin | admin    | Admin@123 |

Note: The admin user is seeded during database migration.

---

## 🧪 Running Unit Tests

### Backend Tests (xUnit)

#### Option 1: Using Visual Studio

1. Open Test Explorer:
   - Go to Test → Test Explorer (or press `Ctrl+E, T`)
2. Run All Tests:
   - Click "Run All" button (green play icon)
   - Or press `Ctrl+R, A`
3. Run Specific Tests:
   - Right-click on test class/method → "Run"
4. View Test Results:
   - Test Explorer shows pass/fail status
   - Green ✓ = Passed
   - Red ✗ = Failed

#### Option 2: Using Command Line

```bash
# Navigate to solution directory
cd Backend

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity detailed

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover

# Run specific test project
dotnet test EmployeeManagement.Tests/EmployeeManagement.Tests.csproj

# Run tests matching a filter
dotnet test --filter "FullyQualifiedName~EmployeeServiceTests"

# Run tests from specific class
dotnet test --filter "FullyQualifiedName~RepositoryTests"
```

#### Expected Output:

```
Test run for EmployeeManagement.Tests.dll (.NET 8.0)
Microsoft (R) Test Execution Command Line Tool Version 17.8.0

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    45, Skipped:     0, Total:    45
```

#### View Code Coverage:

After running tests with coverage:

```bash
# Install ReportGenerator (first time only)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"/coverage.opencover.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open report
start coveragereport/index.html
```

---

### Frontend Tests (Jasmine/Karma)

#### Step 1: Navigate to Frontend Directory

```bash
cd Frontend/employee-management-ui
```

#### Step 2: Run Tests

##### Option A: Watch Mode (Development)

```bash
ng test
```

- Opens Chrome browser
- Tests run automatically on file changes
- Shows real-time results

##### Option B: Single Run (CI Mode)

```bash
ng test --watch=false --code-coverage
```

- Runs once and exits
- Generates coverage report

##### Option C: Headless Mode

```bash
ng test --watch=false --browsers=ChromeHeadless --code-coverage
```

- Runs without opening browser
- Good for CI/CD pipelines

#### Step 3: View Test Results

##### Console Output:

```
Chrome Headless 120.0.0.0 (Windows 10): Executed 32 of 32 SUCCESS (2.145 secs / 1.983 secs)
TOTAL: 32 SUCCESS
```

##### Coverage Report:

```bash
# Open coverage report
cd coverage/employee-management-ui
start index.html
```

Or on Mac/Linux:

```bash
open coverage/employee-management-ui/index.html
```

#### Run Specific Test Files:

```bash
# Run specific spec file
ng test --include='/auth.service.spec.ts'

# Run tests matching pattern
ng test --include='/services/*.spec.ts'

# Run component tests only
ng test --include='/components//*.spec.ts'
```

#### Expected Test Coverage:

| Category     | Statements | Branches | Functions | Lines |
| :----------- | :--------- | :------- | :-------- | :---- |
| Services     | > 90%      | > 85%    | > 90%     | > 90% |
| Components   | > 85%      | > 80%    | > 85%     | > 85% |
| Guards       | > 95%      | > 90%    | > 95%     | > 95% |
| Interceptors | > 90%      | > 85%    | > 90%     | > 90% |

---

## 📊 Test Summary

### Backend Test Coverage:

| Test Category    | Tests | Description                    |
| :--------------- | :---- | :----------------------------- |
| Repository Tests | 9     | CRUD operations, queries       |
| Service Tests    | 12    | Business logic, validation     |
| Controller Tests | 6     | HTTP endpoints, responses      |
| Validator Tests  | 4     | Input validation rules         |
| Total            | 31+   | Comprehensive backend coverage |

### Frontend Test Coverage:

| Test Category         | Tests | Description                     |
| :-------------------- | :---- | :------------------------------ |
| Service Tests         | 12    | HTTP calls, authentication      |
| Component Tests       | 10    | UI logic, user interactions     |
| Guard Tests           | 4     | Route protection                |
| Form Validation Tests | 6     | Reactive forms, validators      |
| Total                 | 32+   | Comprehensive frontend coverage |

---

## 📚 API Documentation

### Swagger UI:

Access: `https://localhost:7001/swagger`

### Main Endpoints:

#### Authentication

```
POST   /api/auth/login       - User login
POST   /api/auth/register    - User registration
```

#### Employees

```
GET    /api/employees                    - Get all employees (paginated)
GET    /api/employees/{id}               - Get employee by ID
POST   /api/employees                    - Create employee (Admin/HR)
PUT    /api/employees/{id}               - Update employee (Admin/HR)
DELETE /api/employees/{id}               - Delete employee (Admin)
```

#### Departments

```
GET    /api/departments                  - Get all departments
GET    /api/departments/{id}             - Get department by ID
POST   /api/departments                  - Create department (Admin)
PUT    /api/departments/{id}             - Update department (Admin)
DELETE /api/departments/{id}             - Delete department (Admin)
```

#### Designations

```
GET    /api/designations                 - Get all designations
GET    /api/designations/{id}            - Get designation by ID
POST   /api/designations                 - Create designation (Admin)
PUT    /api/designations/{id}            - Update designation (Admin)
DELETE /api/designations/{id}            - Delete designation (Admin)
```

### Testing API with Swagger:

1. Navigate to Swagger UI
2. Click "Authorize" button (top right)
3. Login using `/api/auth/login` endpoint
4. Copy the token from response
5. Enter: `Bearer <your-token>` in authorization dialog
6. Click "Authorize"
7. Test other endpoints

---

## 🔧 Troubleshooting

### Backend Issues

#### 1. Database Connection Errors

Error: `Cannot open database "EmployeeManagementDB"`

Solution:

```bash
# Verify SQL Server is running
sqllocaldb info mssqllocaldb
sqllocaldb start mssqllocaldb

# Re-run migrations
dotnet ef database update --project EmployeeManagement.Infrastructure --startup-project EmployeeManagement.API
```

#### 2. Migration Errors

Error: `Build failed`

Solution:

```bash
# Clean and rebuild
dotnet clean
dotnet build
dotnet ef database update
```

#### 3. Port Already in Use

Error: `Address already in use`

Solution:

- Open `Properties/launchSettings.json`
- Change port numbers in `applicationUrl`
- Update frontend `environment.ts` accordingly

#### 4. Missing NuGet Packages

Solution:

```bash
dotnet restore
dotnet build
```

### Frontend Issues

#### 1. Node Modules Issues

Error: `Cannot find module '@angular/core'`

Solution:

```bash
# Delete node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

#### 2. Port 4200 Already in Use

Solution:

```bash
# Use different port
ng serve --port 4300
```

#### 3. CORS Errors

Error: `Access-Control-Allow-Origin`

Solution:

- Verify backend CORS configuration in `Program.cs`
- Ensure frontend URL matches CORS policy
- Check `environment.ts` API URL

#### 4. Authentication Errors

Error: `401 Unauthorized`

Solution:

- Clear browser localStorage
- Login again
- Check token expiration (24 hours)
- Verify JWT configuration in backend

#### 5. API Connection Errors

Error: `ERR_CONNECTION_REFUSED`

Solution:

```bash
# Verify backend is running
# Check backend console for errors
# Verify API URL in environment.ts
```

---

## 🏗️ Architecture Overview

### Backend: Clean Architecture

```
┌─────────────────────────────────────────┐
│          Presentation Layer             │
│         (API Controllers)               │
├─────────────────────────────────────────┤
│        Application Layer                │
│   (Services, DTOs, Validators)          │
├─────────────────────────────────────────┤
│        Infrastructure Layer             │
│  (EF Core, Repositories, External)      │
├─────────────────────────────────────────┤
│          Domain Layer                   │
│    (Entities, Enums, Interfaces)        │
└─────────────────────────────────────────┘
```

Key Principles:

- ✅ Dependency Inversion
- ✅ Separation of Concerns
- ✅ Repository Pattern
- ✅ Unit of Work Pattern
- ✅ SOLID Principles

### Frontend: Component-Based Architecture

```
┌─────────────────────────────────────────┐
│          Components                     │
│     (Smart & Presentational)            │
├─────────────────────────────────────────┤
│          Services                       │
│      (HTTP, Business Logic)             │
├─────────────────────────────────────────┤
│       Guards & Interceptors             │
│    (Auth, Error Handling)               │
├─────────────────────────────────────────┤
│          Models                         │
│      (TypeScript Interfaces)            │
└─────────────────────────────────────────┘
```

---

## 🔒 Security Features

- ✅ JWT Bearer Token Authentication
- ✅ Role-Based Authorization
- ✅ Password Hashing (BCrypt)
- ✅ HTTPS enforcement
- ✅ CORS configuration
- ✅ Input validation (FluentValidation)
- ✅ SQL Injection prevention (EF Core parameterized queries)
- ✅ XSS protection (Angular sanitization)

---

## 📈 Performance Features

- ✅ Async/Await throughout
- ✅ Pagination for large datasets
- ✅ Database query optimization
- ✅ HTTP response caching
- ✅ Lazy loading (Angular modules)
- ✅ Performance middleware logging
- ✅ Connection pooling

---

## 📝 Logging

### Backend Logs:

Location: `Backend/EmployeeManagement.API/Logs/`

Files:

- `log-YYYYMMDD.txt` - Daily log files

View logs:

```bash
tail -f Logs/log-20260207.txt
```

### Log Levels:

- Information - Normal operations
- Warning - Potential issues, slow queries
- Error - Exceptions, failures
- Fatal - Application crashes

---

## 🚀 Deployment Considerations

### Backend:

- Update `appsettings.Production.json`
- Use secure JWT keys
- Configure production database
- Enable HTTPS
- Set up logging infrastructure
- Configure CORS for production domains

### Frontend:

```bash
ng build --configuration production
```

- Update `environment.prod.ts` with production API URL
- Enable production mode
- Optimize bundle size
- Configure CDN for static assets

---

## 👥 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

---

## 📄 License

This project is licensed under the MIT License.

---

## 📞 Support

For issues and questions:

- Create an issue on GitHub
- Email: support@yourcompany.com

---

## ✅ Quick Start Checklist

- [ ] Clone repository
- [ ] Install prerequisites (.NET 8, Node.js, SQL Server)
- [ ] Open backend solution in Visual Studio
- [ ] Restore NuGet packages
- [ ] Configure connection string
- [ ] Run database migrations
- [ ] Start backend API
- [ ] Navigate to frontend directory
- [ ] Run `npm install`
- [ ] Start Angular dev server
- [ ] Login with default credentials
- [ ] Run backend tests
- [ ] Run frontend tests

---

## 🎉 Success!

If you've reached this point and everything works:

- ✅ Backend API running on `https://localhost:7001`
- ✅ Frontend UI running on `http://localhost:4200`
- ✅ Database created with seed data
- ✅ All tests passing
- ✅ Able to login with admin credentials

You're all set! Start exploring the Employee Management System! 🚀

---

Last Updated: February 7, 2026
Version: 1.0.0
Authors: Your Development Team
