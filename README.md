# UsersFunctionApp

An Azure Functions application built with .NET 8 following Hexagonal Architecture and Domain-Driven Design (DDD) principles for user management with Brazilian CPF validation.

## 🏗️ Architecture

The project follows a well-defined layered architecture:

```
src/
├── domain/           # Business rules and entities
├── application/      # Use cases and DTOs
├── functions/        # Primary adapters (HTTP triggers)
└── infrastructure/   # Secondary adapters and middlewares
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
- **GlobalExceptionHandler** - Middleware for global exception handling

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

### Test the API
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

## 🎯 Applied Principles

- **Hexagonal Architecture**: Clear separation of responsibilities
- **Domain-Driven Design**: Rich domain modeling
- **SOLID**: Object-oriented design principles
- **Dependency Injection**: Inversion of control
- **Exception Handling**: Centralized error handling
- **Value Objects**: Encapsulation of validation rules

## 📁 File Structure

```
UsersFunctionApp/
├── src/
│   ├── application/
│   │   ├── dto/
│   │   │   └── UserRequestDTO.cs
│   │   └── service/
│   │       └── UserServiceImpl.cs
│   ├── domain/
│   │   ├── entity/
│   │   │   └── User.cs
│   │   ├── exception/
│   │   │   └── DomainException.cs
│   │   ├── service/
│   │   │   └── IUserService.cs
│   │   └── vo/
│   │       └── Cpf.cs
│   ├── functions/
│   │   └── CreateUser.cs
│   └── infrastructure/
│       └── GlobalExceptionHandler.cs
├── Program.cs
├── host.json
├── local.settings.json
└── UsersFunctionApp.csproj
```

## 🔍 Technical Details

### Exception Middleware
The `GlobalExceptionHandler` intercepts all exceptions and:
- Catches `DomainException` returning 400 Bad Request
- Catches general exceptions returning 500 Internal Server Error
- Formats standardized JSON responses
- Logs structured information

### Dependency Injection
Configured in `Program.cs` to register:
- Domain services
- Global middleware
- Azure Functions configurations

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