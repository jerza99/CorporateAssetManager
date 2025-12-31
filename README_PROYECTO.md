# 📦 Corporate Asset Manager - Documentación del Proyecto

## 🎯 Descripción General

**Corporate Asset Manager** es una aplicación web ASP.NET Core MVC para la gestión integral de activos corporativos, empleados, asignaciones y tickets de soporte.

**Versión:** 1.0  
**Framework:** .NET 10.0  
**Arquitectura:** MVC (Model-View-Controller)  
**Base de Datos:** SQL Server

---

## 📋 Tabla de Contenidos

1. [Arquitectura](#arquitectura)
2. [Tecnologías Utilizadas](#tecnologías-utilizadas)
3. [Estructura del Proyecto](#estructura-del-proyecto)
4. [Configuración](#configuración)
5. [Controladores](#controladores)
6. [Vistas](#vistas)
7. [Autenticación y Autorización](#autenticación-y-autorización)
8. [Modelos](#modelos)
9. [Desarrollo](#desarrollo)
10. [Despliegue](#despliegue)

---

## 🏗️ Arquitectura

### Patrón de Diseño
- **MVC (Model-View-Controller)**: Separación clara de responsabilidades
- **Repository Pattern**: Acceso a datos a través de Entity Framework Core
- **Identity Framework**: Sistema de autenticación y autorización

### Capas de la Aplicación

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│  (Views, Controllers, Razor Pages)  │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│         Business Logic Layer         │
│      (Models, ViewModels, Services)    │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│          Data Access Layer           │
│   (DbContext, Entity Framework)      │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│          Database Layer              │
│          (SQL Server)                │
└─────────────────────────────────────┘
```

---

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 10.0** - Framework principal
- **ASP.NET Core MVC** - Framework web
- **Entity Framework Core 10.0.1** - ORM para acceso a datos
- **ASP.NET Core Identity 10.0.1** - Sistema de autenticación
- **SQL Server** - Base de datos relacional

### Frontend
- **Bootstrap 5.3.3** - Framework CSS
- **jQuery** - Biblioteca JavaScript
- **jQuery Validation** - Validación del lado del cliente
- **Bootstrap Icons** - Iconografía
- **Razor Pages** - Motor de vistas

### Herramientas de Desarrollo
- **Entity Framework Tools** - Migraciones de base de datos
- **Visual Studio Code Generation** - Scaffolding de código

---

## 📁 Estructura del Proyecto

```
CorporateAssetManager/
├── Areas/
│   └── Identity/
│       └── Pages/
│           └── Account/          # Páginas de autenticación (Login, Register, etc.)
├── Controllers/                   # Controladores MVC
│   ├── AssetsController.cs
│   ├── AssetAssignmentsController.cs
│   ├── DashboardController.cs
│   ├── EmployeesController.cs
│   └── HomeController.cs
├── Data/
│   └── ApplicationDbContext.cs   # Contexto de Entity Framework
├── Migrations/                    # Migraciones de base de datos
├── Models/                        # Modelos de dominio
│   ├── ApplicationUser.cs
│   ├── Asset.cs
│   ├── Employee.cs
│   ├── Department.cs
│   ├── Ticket.cs
│   └── ... (22 modelos en total)
├── Views/                        # Vistas Razor
│   ├── Assets/
│   ├── Employees/
│   ├── AssetAssignments/
│   ├── Dashboard/
│   ├── Home/
│   └── Shared/
├── wwwroot/                      # Archivos estáticos
│   ├── css/
│   ├── js/
│   └── images/
├── appsettings.json              # Configuración de la aplicación
├── Program.cs                    # Punto de entrada y configuración
└── CorporateAssetManager.csproj  # Archivo de proyecto
```

---

## ⚙️ Configuración

### 1. Cadena de Conexión

**Archivo:** `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CorporateAssetsDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Configuración para diferentes entornos:**
- **Development:** `appsettings.Development.json`
- **Production:** Configurar en variables de entorno o Azure Key Vault

### 2. Configuración en Program.cs

```csharp
// Configuración de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuración de MVC
builder.Services.AddControllersWithViews();
```

### 3. Pipeline de Middleware

El orden del middleware es crítico:

```csharp
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();      // Debe ir ANTES de Authorization
app.UseAuthorization();
app.MapRazorPages();          // Debe ir ANTES de ControllerRoute
app.MapStaticAssets();         // Debe ir ANTES de ControllerRoute
app.MapControllerRoute(...);  // Debe ir AL FINAL
```

---

## 🎮 Controladores

### HomeController
**Ruta:** `/`  
**Propósito:** Página principal y redirección a login

**Acciones:**
- `Index()` - Página principal (redirige a login si no está autenticado)
- `Privacy()` - Página de privacidad
- `Error()` - Página de error

**Características:**
- Verifica si el usuario está autenticado
- Redirige a `/Account/Login` si no hay sesión activa

---

### EmployeesController
**Ruta:** `/Employees`  
**Autorización:** Requiere autenticación `[Authorize]`  
**Propósito:** Gestión completa de empleados

**Acciones:**
- `Index()` - Lista todos los empleados
- `Details(int? id)` - Detalles de un empleado
- `Create()` - Formulario de creación
- `Create(Employee employee)` - POST: Crea un nuevo empleado
- `Edit(int? id)` - Formulario de edición
- `Edit(int id, Employee employee)` - POST: Actualiza un empleado
- `Delete(int? id)` - Confirmación de eliminación
- `DeleteConfirmed(int id)` - POST: Elimina un empleado

**Características:**
- CRUD completo de empleados
- Validación de modelos
- Manejo de errores

---

### AssetsController
**Ruta:** `/Assets`  
**Autorización:** Requiere autenticación `[Authorize]`  
**Propósito:** Gestión completa de activos

**Acciones:**
- `Index()` - Lista todos los activos
- `Details(int? id)` - Detalles de un activo
- `Create()` - Formulario de creación
- `Create(Asset asset)` - POST: Crea un nuevo activo
- `Edit(int? id)` - Formulario de edición
- `Edit(int id, Asset asset)` - POST: Actualiza un activo
- `Delete(int? id)` - Confirmación de eliminación
- `DeleteConfirmed(int id)` - POST: Elimina un activo

**Características:**
- CRUD completo de activos
- Relación con HardwareSpecs (1:1)
- Gestión de estados y ubicaciones

---

### AssetAssignmentsController
**Ruta:** `/AssetAssignments`  
**Autorización:** Requiere autenticación `[Authorize]`  
**Propósito:** Gestión de asignaciones de activos a empleados

**Acciones:**
- `Index()` - Lista todas las asignaciones
- `Details(int? id)` - Detalles de una asignación
- `Create()` - Formulario de creación
- `Create(AssetAssignment assignment)` - POST: Crea una nueva asignación
- `Edit(int? id)` - Formulario de edición
- `Edit(int id, AssetAssignment assignment)` - POST: Actualiza una asignación
- `Delete(int? id)` - Confirmación de eliminación
- `DeleteConfirmed(int id)` - POST: Elimina una asignación

**Nota:** Esta tabla es legacy. Los nuevos registros deben usar `AssetAssignmentHistory`.

---

### DashboardController
**Ruta:** `/Dashboard`  
**Autorización:** Requiere autenticación `[Authorize]`  
**Propósito:** Panel de control principal

**Acciones:**
- `Index()` - Vista del dashboard

**Características:**
- Panel centralizado de información
- Estadísticas y resúmenes (a implementar)

---

## 🎨 Vistas

### Estructura de Vistas

```
Views/
├── _ViewImports.cshtml          # Imports globales
├── _ViewStart.cshtml            # Layout por defecto
├── Shared/
│   ├── _Layout.cshtml           # Layout principal
│   ├── _Layout.cshtml.css       # Estilos del layout
│   ├── _LoginPartial.cshtml     # Partial view de login
│   ├── _Footer.cshtml           # Footer
│   └── Error.cshtml              # Página de error
├── Home/
│   ├── Index.cshtml             # Página principal
│   └── Privacy.cshtml           # Página de privacidad
├── Employees/
│   ├── Index.cshtml             # Lista de empleados
│   ├── Create.cshtml            # Crear empleado
│   ├── Edit.cshtml              # Editar empleado
│   ├── Details.cshtml           # Detalles de empleado
│   └── Delete.cshtml            # Confirmar eliminación
├── Assets/
│   ├── Index.cshtml             # Lista de activos
│   ├── Create.cshtml            # Crear activo
│   ├── Edit.cshtml              # Editar activo
│   ├── Details.cshtml           # Detalles de activo
│   └── Delete.cshtml            # Confirmar eliminación
├── AssetAssignments/
│   └── ... (vistas similares)
└── Dashboard/
    └── Index.cshtml             # Dashboard principal
```

### Layout Principal

**Archivo:** `Views/Shared/_Layout.cshtml`

**Características:**
- Bootstrap 5.3.3
- Navegación principal con menús:
  - Employees
  - Assets
  - AssetAssignments
- Partial view de login (`_LoginPartial`)
- Footer personalizado
- Scripts de jQuery y Bootstrap

---

## 🔐 Autenticación y Autorización

### ASP.NET Core Identity

**Modelo de Usuario:** `ApplicationUser` (hereda de `IdentityUser`)

**Campos Adicionales:**
- `FullName` - Nombre completo
- `Phone` - Teléfono
- `Age` - Edad
- `Department` - Departamento
- `JobTitle` - Cargo

### Configuración de Identity

```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

**Características:**
- Confirmación de cuenta deshabilitada (para desarrollo)
- Almacenamiento en `ApplicationDbContext`
- Soporte para roles (preparado para futuras implementaciones)

### Páginas de Identity

**Ubicación:** `Areas/Identity/Pages/Account/`

**Páginas Disponibles:**
- `Login.cshtml` - Inicio de sesión
- `Register.cshtml` - Registro de nuevos usuarios
- `Logout.cshtml` - Cerrar sesión
- `ForgotPassword.cshtml` - Recuperación de contraseña
- `ResetPassword.cshtml` - Restablecer contraseña

### Autorización

**Atributos de Autorización:**
- `[Authorize]` - Requiere autenticación
- `[AllowAnonymous]` - Permite acceso sin autenticación

**Uso en Controladores:**
```csharp
[Authorize]
public class EmployeesController : Controller
{
    // Todas las acciones requieren autenticación
}
```

---

## 📊 Modelos

### Modelos Principales

El proyecto contiene **22 modelos** organizados en categorías:

#### 1. Autenticación
- `ApplicationUser` - Usuario del sistema (Identity)

#### 2. Lookup (Catálogos)
- `Modality` - Modalidades de trabajo
- `EmploymentStatus` - Estados de empleo
- `AssetType` - Tipos de activos
- `AssetStatusLookup` - Estados de activos
- `LocationType` - Tipos de ubicación
- `TicketType` - Tipos de tickets
- `TicketStatus` - Estados de tickets

#### 3. Maestras
- `Department` - Departamentos
- `Location` - Ubicaciones
- `Employee` - Empleados
- `Asset` - Activos
- `HardwareSpec` - Especificaciones de hardware
- `Vendor` - Proveedores

#### 4. Historial/Auditoría
- `AssetAssignmentHistory` - Historial de asignaciones
- `AssetStatusHistory` - Historial de estados

#### 5. Transaccionales
- `Ticket` - Tickets de soporte
- `TicketApproval` - Aprobaciones de tickets
- `TicketComment` - Comentarios de tickets
- `SoftwareLicense` - Licencias de software
- `LicenseAssignment` - Asignaciones de licencias
- `PurchaseOrder` - Órdenes de compra
- `AssetMaintenanceRecord` - Registros de mantenimiento

#### 6. Legacy
- `AssetAssignment` - Asignaciones (mantenido por compatibilidad)

**Ver documentación completa:** `README_BASE_DATOS.md`

---

## 🚀 Desarrollo

### Requisitos Previos

- **.NET 10.0 SDK** o superior
- **SQL Server** (LocalDB, Express, o Full)
- **Visual Studio 2022** o **Visual Studio Code**
- **Entity Framework Core Tools** (incluido en el proyecto)

### Configuración Inicial

#### 1. Clonar el Repositorio
```bash
git clone <repository-url>
cd CorporateAssetManager
```

#### 2. Restaurar Dependencias
```bash
dotnet restore
```

#### 3. Configurar Base de Datos

**Opción A: Usar SQL Server LocalDB**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CorporateAssetsDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

**Opción B: Usar SQL Server Express**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CorporateAssetsDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

#### 4. Aplicar Migraciones
```bash
dotnet ef database update
```

Esto creará la base de datos y aplicará todas las migraciones.

#### 5. Ejecutar la Aplicación
```bash
dotnet run
```

O desde Visual Studio:
- Presionar `F5` o `Ctrl+F5`

La aplicación estará disponible en:
- HTTP: `http://localhost:5154`
- HTTPS: `https://localhost:7007`

### Crear un Usuario Inicial

1. Navegar a `/Identity/Account/Register`
2. Completar el formulario de registro
3. El usuario quedará registrado y podrá iniciar sesión

### Comandos Útiles

#### Migraciones de Base de Datos
```bash
# Crear una nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración (sin aplicar)
dotnet ef migrations remove

# Listar migraciones
dotnet ef migrations list
```

#### Compilación y Ejecución
```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Ejecutar en modo release
dotnet run --configuration Release

# Publicar para producción
dotnet publish -c Release -o ./publish
```

---

## 🔧 Configuración Avanzada

### Variables de Entorno

Para producción, usar variables de entorno en lugar de `appsettings.json`:

```bash
# Windows
set ConnectionStrings__DefaultConnection="Server=..."

# Linux/Mac
export ConnectionStrings__DefaultConnection="Server=..."
```

### Logging

**Configuración en `appsettings.json`:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### HTTPS

La aplicación está configurada para usar HTTPS en desarrollo. Para producción:
- Configurar certificados SSL
- Habilitar HSTS (HTTP Strict Transport Security)
- Configurar redirección HTTP a HTTPS

---

## 📦 Despliegue

### Opción 1: Azure App Service

1. Publicar la aplicación:
```bash
dotnet publish -c Release
```

2. Crear App Service en Azure Portal
3. Configurar cadena de conexión en Configuration
4. Desplegar desde Visual Studio o Azure CLI

### Opción 2: IIS (Windows Server)

1. Instalar .NET 10.0 Hosting Bundle
2. Publicar la aplicación:
```bash
dotnet publish -c Release -o C:\inetpub\wwwroot\CorporateAssetManager
```

3. Configurar sitio web en IIS
4. Configurar Application Pool con .NET CLR Version = "No Managed Code"

### Opción 3: Docker (Futuro)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CorporateAssetManager.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CorporateAssetManager.dll"]
```

---

## 🧪 Testing (Futuro)

### Unit Tests
- Crear proyecto `CorporateAssetManager.Tests`
- Usar xUnit o NUnit
- Mock de DbContext para pruebas

### Integration Tests
- Probar controladores con TestServer
- Probar migraciones de base de datos

---

## 📝 Mejoras Futuras

### Alta Prioridad
- [ ] Implementar autorización basada en roles
- [ ] Agregar capa de servicios/repositorios
- [ ] Implementar manejo global de excepciones
- [ ] Agregar logging estructurado
- [ ] Implementar validación de lógica de negocio

### Media Prioridad
- [ ] Agregar paginación en listas
- [ ] Implementar filtros y ordenamiento
- [ ] Crear DTOs para transferencia de datos
- [ ] Implementar soft delete
- [ ] Agregar campos de auditoría automáticos

### Baja Prioridad
- [ ] Agregar tests unitarios e integración
- [ ] Implementar carga de imágenes a Google Cloud Storage
- [ ] Agregar dashboard con estadísticas
- [ ] Implementar exportación de datos (Excel, PDF)
- [ ] Agregar sistema de notificaciones
- [ ] Crear API REST

---

## 🐛 Solución de Problemas

### Error: "Connection string not found"
**Solución:** Verificar que `appsettings.json` tenga la cadena de conexión configurada.

### Error: "Migration pending"
**Solución:** Ejecutar `dotnet ef database update`

### Error: "Cannot create index because duplicate keys"
**Solución:** Verificar que no haya datos duplicados en campos únicos. Limpiar datos o regenerar códigos únicos.

### Error: "User not authenticated"
**Solución:** Asegurarse de estar registrado e iniciar sesión en `/Identity/Account/Login`

---

## 📚 Recursos Adicionales

- **Documentación de Base de Datos:** Ver `README_BASE_DATOS.md`
- **Entity Framework Core:** https://docs.microsoft.com/ef/core/
- **ASP.NET Core MVC:** https://docs.microsoft.com/aspnet/core/mvc/
- **ASP.NET Core Identity:** https://docs.microsoft.com/aspnet/core/security/authentication/identity

---

## 👥 Contribución

### Estructura de Commits
- `feat:` Nueva funcionalidad
- `fix:` Corrección de bugs
- `docs:` Documentación
- `refactor:` Refactorización de código
- `test:` Tests

### Branching Strategy
- `main` - Código de producción
- `develop` - Desarrollo activo
- `feature/*` - Nuevas funcionalidades
- `bugfix/*` - Correcciones de bugs

---

## 📄 Licencia

*[Especificar licencia del proyecto]*

---

**Última actualización:** 2024-12-31  
**Versión del Proyecto:** 1.0  
**Framework:** .NET 10.0
