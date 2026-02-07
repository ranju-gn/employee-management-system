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

git clone https://github.com/ranju-gn/employee-management-system.git

# Navigate to the directory

cd employee-management-system

### Step 2: Open Solution in Visual Studio

### Step 3: Restore NuGet Packages

Visual Studio will automatically restore packages. If not:

# Option 1: In Visual Studio

Right-click Solution → "Restore NuGet Packages"

# Option 2: Using CLI

dotnet restore

### Step 4: Configure Connection String

1. Open `EmployeeManagement.API/appsettings.json`
2. Update the connection string based on your SQL Server setup:

#### For SQL Server with credentials:

{
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=EmployeeManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
}

### Step 5: Verify JWT Configuration

In `appsettings.json`, ensure JWT settings are present:

{
"Jwt": {
"Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
"Issuer": "EmployeeManagementAPI",
"Audience": "EmployeeManagementClient"
}
}

### Step 6: Build the Solution

# In Visual Studio

Build → Build Solution (Ctrl+Shift+B)

# Or using CLI

dotnet build

---

## 🗄️ Database Setup

### Step 1: Open Package Manager Console

In Visual Studio:

1. Go to Tools → NuGet Package Manager → Package Manager Console
2. Set Default project to: `EmployeeManagement.Infrastructure`

### Step 2: Run Migrations

# Update database

Update-Database

Expected output:

Build started...
Build succeeded.
Applying migration '20260207_InitialCreate'.
Done.

### Step 3: Verify Database Creation

#### Using SSMS:

1. Open SQL Server Management Studio
2. Connect to your server
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

-- Check departments
SELECT * FROM Departments;

-- Check designations
SELECT * FROM Designations;

-- Check default user
SELECT * FROM Users;

You should see:

- 3 Departments: IT, HR, Finance
- 4 Designations: Software Engineer, Senior Software Engineer, Tech Lead, HR Manager
- 1 User: admin (for login)

---

## Frontend Setup

### Step 1: Navigate to Frontend Directory

# From project root
cd employee-management-ui

### Step 2: Install Dependencies

npm install

### Step 3: Verify Angular CLI

ng version

Expected output:

Angular CLI: 18.x.x
Node: 20.x.x
Package Manager: npm 10.x.x

### Step 4: Configure API Endpoint

1. Open `src/environments/environment.ts`
2. Verify the API URL matches your backend:

export const environment = {
  production: false,
  apiUrl: "https://localhost:7089/api",
};

## ▶️ Running the Application

### Step 1: Start Backend API

#### Option A: Using Visual Studio

1. Ensure `EmployeeManagement.API` is set as startup project (should be in bold)
2. Press F5 (Debug) or Ctrl+F5 (Without Debug)
3. Browser will open with Swagger UI: `https://localhost:7089/swagger`

#### Option B: Using Command Line

cd Backend/EmployeeManagement.API
dotnet run

#### Verify Backend is Running:

- Swagger UI should load at: `https://localhost:7089/swagger`
- Console should show:

info: Microsoft.Hosting.Lifetime
      Now listening on: https://localhost:7089
info: Microsoft.Hosting.Lifetime
      Application started.

### Step 2: Start Frontend Application

Open a new terminal/command prompt:

cd Frontend/employee-management-ui
ng serve

#### Verify Frontend is Running:

- Console should show:

 Angular Live Development Server is listening on localhost:4200
✔ Compiled successfully.

- open: `http://localhost:4200`

### Step 3: Access the Application

1. Frontend URL: http://localhost:4200
2. Backend API: https://localhost:7089
3. Swagger Documentation: https://localhost:7089/swagger

---

## Default Credentials

### Login to the Application:

URL: http://localhost:4200/login

| Role  | Username | Password  |
| :---- | :------- | :-------- |
| Admin | admin    | Admin@123 |

Note: The admin user is seeded during database migration.

---

## Running Unit Tests

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

# Navigate to solution directory

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

#### View Code Coverage:

After running tests with coverage:

# Install ReportGenerator (first time only)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"/coverage.opencover.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open report
start coveragereport/index.html

---

### Frontend Tests (Karma)

#### Step 1: Navigate to Frontend Directory

cd employee-management-ui

#### Step 2: Run Tests

##### Option A: Watch Mode (Development)

ng test

- Opens Chrome browser
- Tests run automatically on file changes
- Shows real-time results