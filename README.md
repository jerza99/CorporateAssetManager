# 📦 Corporate Asset Manager

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-blue?style=for-the-badge)

> Sistema integral de gestión de activos corporativos, empleados, asignaciones y tickets de soporte desarrollado con ASP.NET Core MVC.

---

## 🎯 Descripción

**Corporate Asset Manager** es una aplicación web diseñada para gestionar todos los activos de una compañía. El sistema permite llevar un control completo de equipos, empleados, asignaciones, tickets de soporte y mantener un historial detallado de todas las operaciones.

### ✨ Características Principales

- 🏢 **Gestión de Empleados**: Administración completa de información de empleados, departamentos y jerarquías organizacionales
- 💻 **Gestión de Activos**: Control detallado de activos corporativos con especificaciones técnicas y estados
- 📋 **Sistema de Asignaciones**: Historial completo de asignaciones y movimientos de activos
- 🎫 **Tickets de Soporte**: Sistema completo de tickets con aprobaciones, comentarios y seguimiento
- 📊 **Dashboard Centralizado**: Panel de control con estadísticas y resúmenes
- 🔐 **Autenticación y Autorización**: Sistema de seguridad basado en ASP.NET Core Identity
- 📜 **Trazabilidad Completa**: Historial detallado de todos los cambios y movimientos
- 🗄️ **Base de Datos Normalizada**: Arquitectura robusta con tablas Lookup, Maestras, Historial y Transaccionales

---

## 🛠️ Stack Tecnológico

### Backend
- **.NET 10.0** - Framework principal
- **ASP.NET Core MVC** - Framework web
- **Entity Framework Core 10.0.1** - ORM para acceso a datos
- **ASP.NET Core Identity 10.0.1** - Sistema de autenticación

### Base de Datos
- **SQL Server** - Base de datos relacional

### Frontend
- **Bootstrap 5.3.3** - Framework CSS
- **jQuery** - Biblioteca JavaScript
- **Razor Pages** - Motor de vistas

---

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) o superior
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB, Express, o Full)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- [Entity Framework Core Tools](https://docs.microsoft.com/ef/core/cli/dotnet)

---

## 🚀 Instalación Rápida

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/CorporateAssetManager.git
cd CorporateAssetManager
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Configurar Base de Datos

Edita `appsettings.json` y configura tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CorporateAssetsDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Aplicar Migraciones

```bash
dotnet ef database update
```

### 5. Ejecutar la Aplicación

```bash
dotnet run
```

### 6. Crear Usuario Inicial

1. Navega a `/Identity/Account/Register`
2. Completa el formulario de registro
3. Inicia sesión con tus credenciales

---

## 📁 Estructura del Proyecto

```
CorporateAssetManager/
├── Areas/
│   └── Identity/              # Páginas de autenticación
├── Controllers/                # Controladores MVC
│   ├── AssetsController.cs
│   ├── AssetAssignmentsController.cs
│   ├── DashboardController.cs
│   ├── EmployeesController.cs
│   └── HomeController.cs
├── Data/
│   └── ApplicationDbContext.cs # Contexto de Entity Framework
├── Migrations/                 # Migraciones de base de datos
├── Models/                     # Modelos de dominio (22 modelos)
├── Views/                      # Vistas Razor
│   ├── Assets/
│   ├── Employees/
│   ├── AssetAssignments/
│   ├── Dashboard/
│   └── Shared/
├── wwwroot/                    # Archivos estáticos
│   ├── css/
│   ├── js/
│   └── images/
├── appsettings.json            # Configuración
├── Program.cs                  # Punto de entrada
└── CorporateAssetManager.csproj
```

---

## 📚 Documentación

Este proyecto incluye documentación detallada:

- **[📖 Documentación Completa del Proyecto](README_PROYECTO.md)** - Guía técnica completa con arquitectura, controladores, vistas, configuración y más
- **[🗄️ Documentación de Base de Datos](README_BASE_DATOS.md)** - Estructura completa de la base de datos, tablas, relaciones y ejemplos de uso

---

## 🎮 Funcionalidades Principales

### Gestión de Empleados
- CRUD completo de empleados
- Gestión de departamentos y jerarquías
- Estados de empleo y modalidades de trabajo
- Relaciones supervisor-subordinado

### Gestión de Activos
- Registro completo de activos corporativos
- Especificaciones técnicas detalladas (HardwareSpecs)
- Control de estados y ubicaciones
- Gestión de garantías y fechas de compra

### Sistema de Asignaciones
- Asignación de activos a empleados
- Historial completo de movimientos
- Transferencias entre empleados/ubicaciones
- Devoluciones y cambios de estado

### Tickets de Soporte
- Creación y seguimiento de tickets
- Sistema de aprobaciones
- Comentarios y actualizaciones
- Relación con activos y empleados

---

## 🏗️ Arquitectura

El proyecto sigue una arquitectura **MVC (Model-View-Controller)** con las siguientes capas:

```
┌─────────────────────────────────────┐
│    Presentation Layer (MVC)         │
│  (Views, Controllers, Razor Pages)  │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Business Logic Layer              │
│  (Models, ViewModels, Services)      │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Data Access Layer                 │
│  (DbContext, Entity Framework)       │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Database Layer                    │
│  (SQL Server)                        │
└─────────────────────────────────────┘
```

---

## 🔧 Comandos Útiles

### Migraciones de Base de Datos

```bash
# Crear una nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración (sin aplicar)
dotnet ef migrations remove
```

### Desarrollo

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

## 🗄️ Base de Datos

La base de datos está organizada en **4 categorías principales**:

1. **Tablas Lookup (Catálogos)**: Valores predefinidos (Estados, Tipos, Modalidades)
2. **Tablas Maestras**: Entidades principales (Empleados, Activos, Departamentos)
3. **Tablas de Historial/Auditoría**: Trazabilidad completa (Asignaciones, Estados)
4. **Tablas Transaccionales**: Procesos de negocio (Tickets, Órdenes de Compra, Mantenimientos)

**Total**: 22 modelos organizados en una estructura normalizada y escalable.

Para más detalles, consulta la [Documentación de Base de Datos](README_BASE_DATOS.md).

---

## 🚧 Roadmap

### Alta Prioridad
- [ ] Implementar autorización basada en roles
- [ ] Agregar capa de servicios/repositorios
- [ ] Implementar manejo global de excepciones
- [ ] Agregar logging estructurado

### Media Prioridad
- [ ] Agregar paginación en listas
- [ ] Implementar filtros y ordenamiento
- [ ] Crear DTOs para transferencia de datos
- [ ] Implementar soft delete

### Baja Prioridad
- [ ] Agregar tests unitarios e integración
- [ ] Implementar carga de imágenes a Google Cloud Storage
- [ ] Agregar dashboard con estadísticas
- [ ] Implementar exportación de datos (Excel, PDF)

---

## 🤝 Contribución

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'feat: Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Estructura de Commits

- `feat:` Nueva funcionalidad
- `fix:` Corrección de bugs
- `docs:` Documentación
- `refactor:` Refactorización de código
- `test:` Tests

---

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

**Nota:** La licencia MIT permite que otros usen, modifiquen y distribuyan tu código, pero **tú sigues siendo el propietario y autor original**. El copyright siempre te pertenece.

---

## 👤 Autor

**Jerzayl Balladares Badillo**

- GitHub: [@tu-usuario](https://github.com/jerza99)
- Email: jerzayl99@gmail.com

---

## 🙏 Agradecimientos

- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Bootstrap](https://getbootstrap.com/)

---

<div align="center">

**⭐ Si este proyecto te resulta útil, considera darle una estrella ⭐**

</div>
