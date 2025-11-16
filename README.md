# UsersFunctionApp

An Azure Functions application built with .NET 8 following Hexagonal Architecture and Domain-Driven Design (DDD) principles for user management with Brazilian CPF validation.

## 🏗️ Architecture

The project follows a well-defined layered architecture with Java-style structure:

```
src/
├── main/                    # Source code
│   ├── domain/              # Business rules and entities
│   ├── application/         # Use cases and DTOs
│   ├── functions/           # Primary adapters (HTTP triggers)
│   ├── infraestructure/     # Secondary adapters and middlewares
│   └── Program.cs           # Application entry point
└── test/                    # Test suite (34 tests)
    ├── unit/                # Unit tests (26 tests)
    │   ├── CpfTests.cs      # CPF validation tests
    │   ├── UserTests.cs     # User entity tests
    │   ├── UserServiceTests.cs # Service layer tests
    │   └── UserMapperTests.cs # Mapper tests
    ├── integration/         # Integration tests (4 tests)
    │   └── ServiceIntegrationTests.cs
    ├── e2e/                 # End-to-End tests (4 tests)
    │   └── ApiTests.cs      # HTTP API tests
    └── UsersFunctionApp.Tests.csproj
```

### Domain Layer
- **Entity**: `User` - Main entity with business validations
- **Value Object**: `Cpf` - Brazilian CPF validation and formatting with check digit calculation
- **Port**: `IUserService` - Interface for domain services
- **Exception**: `DomainException` - Domain-specific exceptions

### Application Layer
- **DTO**: `UserRequestDTO` - Record for HTTP requests
- **Service**: `UserServiceImpl` - Use case implementations

### Functions Layer
- **CreateUser** - Azure Function for user creation via HTTP

### Infrastructure Layer
- **GlobalExceptionHandler** - Middleware for global exception handling (configured but handled in function)

## 🚀 Features

### User Creation
- **Endpoint**: `POST /api/create-user`
- **Validations**:
  - Name is required
  - Age cannot be negative
  - CPF must be valid (11 digits with check digit verification)

### CPF Validation
The `Cpf` Value Object implements:
- Normalization (removes non-numeric characters)
- Format validation (11 digits)
- Invalid sequence validation (111.111.111-11)
- Check digit calculation and verification
- Automatic formatting (xxx.xxx.xxx-xx)

## 🛠️ Technologies

- **.NET 8**
- **Azure Functions v4**
- **Microsoft.Azure.Functions.Worker** (Isolated Process)
- **ASP.NET Core Integration**
- **Application Insights**

## 📦 Project Setup

The project was initialized with:
```bash
func init --worker-runtime dotnet-isolated
func new --Name CreateUser --template "Http trigger"
```

## 📋 Requirements

- **.NET SDK 8.0**
- **Azure Functions Core Tools v4**
- **Node.js** (for Azure Functions Core Tools installation)

### Azure Functions Core Tools Installation
```bash
npm install -g azure-functions-core-tools@4 --unsafe-perm true
```

## 🔧 How to Run

### Prerequisites
- .NET 8 SDK
- Azure Functions Core Tools v4
- Visual Studio or VS Code

### Run Locally
```bash
func start
```

### Run Tests

#### Execute All Tests (34 tests)
```bash
# From root directory
dotnet test src/test
```

#### Execute Tests by Category
```bash
# Unit Tests Only (26 tests)
dotnet test src/test --filter "FullyQualifiedName~unit"

# Integration Tests Only (4 tests)
dotnet test src/test --filter "FullyQualifiedName~integration"

# E2E Tests Only (4 tests) - Requires server running
dotnet test src/test --filter "FullyQualifiedName~e2e"

# Unit + Integration Tests (30 tests) - Skip E2E
dotnet test src/test --filter "FullyQualifiedName!~e2e"
```

#### Execute Specific Test Files
```bash
# CPF validation tests
dotnet test src/test/unit/CpfTests.cs

# User entity tests
dotnet test src/test/unit/UserTests.cs

# Service layer tests
dotnet test src/test/unit/UserServiceTests.cs

# Mapper tests
dotnet test src/test/unit/UserMapperTests.cs

# Integration tests
dotnet test src/test/integration/ServiceIntegrationTests.cs

# E2E API tests
dotnet test src/test/e2e/ApiTests.cs
```

#### Execute Tests with Verbose Output
```bash
# Detailed test output
dotnet test src/test --verbosity normal

# Show test names as they run
dotnet test src/test --logger "console;verbosity=detailed"
```

#### Execute E2E Tests (Requires Running Server)
```bash
# Terminal 1: Start Azure Functions
func start

# Terminal 2: Run E2E tests
dotnet test src/test --filter "FullyQualifiedName~e2e"
```

### Test the API

#### Prerequisites for E2E Tests
Before running E2E tests, start the Azure Functions server:
```bash
func start
```

#### Manual API Testing
**Local:**
```bash
POST http://localhost:7071/api/create-user
Content-Type: application/json

{
  "Name": "João Silva",
  "Age": 30,
  "Cpf": "12345678901"
}
```

**Azure:**
```bash
POST https://usercore-ddd-funcapp.azurewebsites.net/api/create-user
Content-Type: application/json

{
  "Name": "João Silva",
  "Age": 30,
  "Cpf": "12345678901"
}
```

## 📋 Response Structure

### Success (200 OK)
```json
{
  "Id": "guid-gerado",
  "Name": "João Silva",
  "Age": 30,
  "Cpf": "123.456.789-01"
}
```

### Validation Error (400 Bad Request)
```json
{
  "Code": 400,
  "Message": "Invalid CPF",
  "Timestamp": "2024-01-01T10:00:00Z"
}
```

### Internal Error (500 Internal Server Error)
```json
{
  "Code": 500,
  "Message": "Internal server error",
  "Timestamp": "2024-01-01T10:00:00Z"
}
```

## 🧪 Test Suite

The project includes comprehensive tests following the test pyramid pattern:

### Test Coverage (34 Total Tests)

#### Unit Tests (26 tests)
- **CpfTests** (15 tests): Value Object validation and CPF algorithm
- **UserTests** (6 tests): Entity business rules and validations
- **UserServiceTests** (4 tests): Application service layer logic
- **UserMapperTests** (1 test): DTO mapping functionality

#### Integration Tests (4 tests)
- **ServiceIntegrationTests**: Complete service pipeline testing
- Tests the integration between domain, application, and infrastructure layers

#### E2E Tests (4 tests)
- **ApiTests**: HTTP API endpoint testing
- Tests complete request/response cycle
- Requires Azure Functions server running locally

### Test Categories
- ✅ **Unit**: Fast, isolated, no dependencies
- ✅ **Integration**: Service layer integration
- ✅ **E2E**: Full HTTP API testing

### Test Technologies
- **xUnit**: Test framework
- **FluentAssertions**: Readable assertions
- **Theory/InlineData**: Parameterized tests
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **HttpClient**: E2E API testing

## 🎯 Applied Principles

- **Hexagonal Architecture**: Clear separation of responsibilities
- **Domain-Driven Design**: Rich domain modeling
- **SOLID**: Object-oriented design principles
- **Dependency Injection**: Inversion of control
- **Exception Handling**: Centralized error handling
- **Value Objects**: Encapsulation of validation rules
- **Test Pyramid**: Comprehensive test coverage (Unit > Integration > E2E)
- **Clean Code**: Readable and maintainable code structure

## 📁 File Structure

```
UsersFunctionApp/
├── src/
│   ├── main/                           # Source code
│   │   ├── application/                # Application layer
│   │   │   ├── dto/                    # Data Transfer Objects
│   │   │   │   ├── UserMapper.cs       # DTO mapping logic
│   │   │   │   ├── UserRequestDTO.cs   # HTTP request DTO
│   │   │   │   └── UserResponseDTO.cs  # HTTP response DTO
│   │   │   └── service/                # Application services
│   │   │       └── UserServiceImpl.cs  # User service implementation
│   │   ├── domain/                     # Domain layer
│   │   │   ├── entity/                 # Domain entities
│   │   │   │   └── User.cs             # User entity with business rules
│   │   │   ├── exception/              # Domain exceptions
│   │   │   │   └── DomainException.cs  # Custom domain exception
│   │   │   ├── service/                # Domain service interfaces
│   │   │   │   └── IUserService.cs     # User service contract
│   │   │   └── vo/                     # Value Objects
│   │   │       └── Cpf.cs              # CPF value object with validation
│   │   ├── functions/                  # Azure Functions (Primary adapters)
│   │   │   └── CreateUser.cs           # HTTP trigger function
│   │   ├── infraestructure/            # Infrastructure layer
│   │   │   └── GlobalExceptionHandler.cs # Global exception middleware
│   │   └── Program.cs                  # Application entry point
│   └── test/                           # Test suite
│       ├── unit/                       # Unit tests (26 tests)
│       │   ├── CpfTests.cs             # CPF validation tests (15 tests)
│       │   ├── UserTests.cs            # User entity tests (6 tests)
│       │   ├── UserServiceTests.cs     # Service layer tests (4 tests)
│       │   └── UserMapperTests.cs      # Mapper tests (1 test)
│       ├── integration/                # Integration tests (4 tests)
│       │   └── ServiceIntegrationTests.cs # Service pipeline tests
│       ├── e2e/                        # End-to-End tests (4 tests)
│       │   └── ApiTests.cs             # HTTP API tests
│       └── UsersFunctionApp.Tests.csproj # Test project file
├── Properties/
│   └── launchSettings.json             # Launch configuration
├── .gitignore                          # Git ignore rules
├── host.json                           # Azure Functions host configuration
├── local.settings.json                 # Local development settings
├── LICENSE                             # MIT License
├── README.md                           # Project documentation
├── UsersFunctionApp.csproj             # Main project file
└── UsersFunctionApp.sln                # Solution file
```

## 🔍 Technical Details

### Exception Handling
Exception handling is implemented directly in the `CreateUser` function with try/catch blocks:
- Catches `DomainException` returning 400 Bad Request with structured JSON
- Validates request body and returns appropriate error messages
- Formats standardized JSON responses with code, message, and timestamp
- Logs structured information using ILogger

### Dependency Injection
Configured in `Program.cs` to register:
- Domain services (`IUserService` → `UserServiceImpl`)
- Application Insights telemetry
- Azure Functions Worker configurations with ASP.NET Core integration

### CPF Value Object
Implements complete validation following official Brazilian rules:
1. Removes non-numeric characters
2. Verifies 11 digits length
3. Rejects repeated sequences
4. Calculates first check digit
5. Calculates second check digit
6. Formats for display

## ☁️ Azure Deployment

### Prerequisites
- Azure CLI installed and logged in
- PowerShell
- .NET 8 SDK

### Deploy to Azure Functions

```powershell
# Set deployment variables
$RG_NAME="usercore-ddd-rg" 
$APP_NAME="usercore-ddd-funcapp"
$LOCATION="eastus" 
$RUNTIME="dotnet-isolated"
$APPINSIGHTS_NAME="${APP_NAME}-ai"
# Generate unique Storage Account name with timestamp
$STORAGE_NAME="usercoreddddsa$(Get-Date -Format 'yyyymmdd')"

# Create Azure resources
az group create -n $RG_NAME -l $LOCATION

az storage account create -n $STORAGE_NAME -l $LOCATION -g $RG_NAME --sku Standard_LRS

az monitor app-insights component create --app $APPINSIGHTS_NAME -l $LOCATION -g $RG_NAME --kind web

az functionapp create -n $APP_NAME -g $RG_NAME -c $LOCATION --storage-account $STORAGE_NAME --runtime $RUNTIME --runtime-version 8 --functions-version 4 --app-insights $APPINSIGHTS_NAME --os-type windows

# Build and deploy
dotnet publish -c Release -o ./publish

Compress-Archive -Path .\publish\* -DestinationPath .\publish.zip -Force

az functionapp deployment source config-zip -g $RG_NAME -n $APP_NAME --src .\publish.zip
```

### Post-Deployment
After deployment, your API will be available at:
```
https://usercore-ddd-funcapp.azurewebsites.net/api/create-user
```

**Note:** To test the API in Postman or Thunder Client after deployment, you'll need to include the `x-functions-key` header. Get the key with:
```bash
az functionapp keys list -g usercore-ddd-rg -n usercore-ddd-funcapp --query 'functionKeys.default' -o tsv
```