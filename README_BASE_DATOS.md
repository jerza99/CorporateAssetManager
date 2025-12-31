# 🗄️ Corporate Asset Manager - Documentación de Base de Datos

## 🎯 Descripción General

Esta base de datos está diseñada para gestionar activos corporativos (equipos, licencias de software, tickets de soporte, etc.) con un sistema completo de trazabilidad, historial y auditoría.

**Arquitectura:** Base de datos relacional con Entity Framework Core  
**Motor:** SQL Server  
**Patrón:** Normalización con tablas Lookup, Maestras, Historial y Transaccionales  
**Versión:** 3.0

---

## 📋 Tabla de Contenidos

1. [Estructura de la Base de Datos](#estructura-de-la-base-de-datos)
2. [Tablas Lookup (Catálogos)](#1-tablas-lookup-catálogos)
3. [Tablas Maestras](#2-tablas-maestras)
4. [Tablas de Historial/Auditoría](#3-tablas-de-historialauditoría)
5. [Tablas Transaccionales](#4-tablas-transaccionales)
6. [Relaciones entre Tablas](#relaciones-entre-tablas)
7. [Índices Únicos](#índices-únicos)
8. [Flujos de Trabajo](#flujos-de-trabajo)
9. [Ejemplos de Uso](#ejemplos-de-uso)

---

## 📊 Estructura de la Base de Datos

La base de datos se organiza en **4 categorías principales**:

### 1. **Tablas Lookup (Catálogos)** 📋
Tablas de referencia que contienen valores predefinidos y raramente cambian.

### 2. **Tablas Maestras** 🏢
Tablas principales que contienen entidades de negocio (Empleados, Activos, Departamentos, etc.).

### 3. **Tablas de Historial/Auditoría** 📜
Registran todos los cambios y movimientos para trazabilidad completa.

### 4. **Tablas Transaccionales** 💼
Gestionan procesos de negocio (Tickets, Órdenes de Compra, Mantenimientos).

---

## 📋 1. TABLAS LOOKUP (Catálogos)

Estas tablas almacenan valores predefinidos que se usan como referencias en otras tablas.

### 1.1 `Modalities` - Modalidades de Trabajo

**Propósito:** Define cómo trabaja un empleado (Remoto, Oficina, Híbrido).

**Estructura:**
```sql
ModalityId (PK, smallint, IDENTITY)  -- ID único
Name (nvarchar(30), NOT NULL, UNIQUE) -- "Remoto", "Oficina", "Híbrido"
Description (nvarchar(200), NULL)    -- Descripción opcional
```

**Datos Iniciales:**
- `1`: "Remoto" - Trabajo completamente remoto
- `2`: "Oficina" - Trabajo en oficina física
- `3`: "Híbrido" - Combinación de remoto y oficina

**Relación:** `Employees.ModalityId` → `Modalities.ModalityId` (Cascade)

---

### 1.2 `EmploymentStatuses` - Estados de Empleo

**Propósito:** Define el estado laboral de un empleado.

**Estructura:**
```sql
StatusId (PK, smallint, IDENTITY)    -- ID único
Name (nvarchar(30), NOT NULL, UNIQUE) -- "Activo", "Inactivo", etc.
```

**Datos Iniciales:**
- `1`: "Activo"
- `2`: "Inactivo"
- `3`: "Licencia"
- `4`: "Suspendido"

**Relación:** `Employees.StatusId` → `EmploymentStatuses.StatusId` (Cascade)

---

### 1.3 `AssetTypes` - Tipos de Activos

**Propósito:** Clasifica los activos por tipo de equipo.

**Estructura:**
```sql
AssetTypeId (PK, smallint, IDENTITY) -- ID único
Name (nvarchar(50), NOT NULL, UNIQUE) -- "Workstation", "Laptop", etc.
```

**Datos Iniciales:**
- `1`: "Workstation"
- `2`: "Laptop"
- `3`: "GPU Externa"
- `4`: "Tableta"
- `5`: "Monitor"
- `6`: "Servidor"

**Relación:** `Assets.AssetTypeId` → `AssetTypes.AssetTypeId` (Cascade)

---

### 1.4 `AssetStatuses` - Estados de Activos

**Propósito:** Define el estado actual de un activo.

**Estructura:**
```sql
StatusId (PK, smallint, IDENTITY)     -- ID único
Name (nvarchar(50), NOT NULL, UNIQUE) -- "Disponible", "Asignado", etc.
```

**Datos Iniciales:**
- `1`: "Disponible"
- `2`: "Asignado"
- `3`: "En Reparación"
- `4`: "Dañado"
- `5`: "Dado de Baja"

**Relaciones:**
- `Assets.CurrentStatusId` → `AssetStatuses.StatusId` (Cascade)
- `AssetStatusHistories.OldStatusId` → `AssetStatuses.StatusId` (Restrict)
- `AssetStatusHistories.NewStatusId` → `AssetStatuses.StatusId` (Restrict)

---

### 1.5 `LocationTypes` - Tipos de Ubicación

**Propósito:** Clasifica las ubicaciones físicas.

**Estructura:**
```sql
LocationTypeId (PK, smallint, IDENTITY) -- ID único
Name (nvarchar(50), NOT NULL, UNIQUE)   -- "Oficina", "Domicilio", etc.
```

**Datos Iniciales:**
- `1`: "Oficina"
- `2`: "Domicilio"
- `3`: "Co-working"

**Relación:** `Locations.LocationTypeId` → `LocationTypes.LocationTypeId` (Cascade)

---

### 1.6 `TicketTypes` - Tipos de Tickets

**Propósito:** Clasifica los tickets de soporte.

**Estructura:**
```sql
TypeId (PK, smallint, IDENTITY)        -- ID único
Name (nvarchar(50), NOT NULL, UNIQUE) -- "Solicitud", "Incidencia"
```

**Datos Iniciales:**
- `1`: "Solicitud"
- `2`: "Incidencia"

**Relación:** `Tickets.TicketTypeId` → `TicketTypes.TypeId` (Cascade)

---

### 1.7 `TicketStatuses` - Estados de Tickets

**Propósito:** Define el estado de un ticket en su ciclo de vida.

**Estructura:**
```sql
StatusId (PK, smallint, IDENTITY)     -- ID único
Name (nvarchar(50), NOT NULL, UNIQUE) -- "Pendiente", "Aprobado", etc.
```

**Datos Iniciales:**
- `1`: "Pendiente"
- `2`: "En Revisión"
- `3`: "Aprobado"
- `4`: "Rechazado"
- `5`: "En Proceso"
- `6`: "Resuelto"
- `7`: "Cerrado"

**Relación:** `Tickets.StatusId` → `TicketStatuses.StatusId` (Cascade)

---

## 🏢 2. TABLAS MAESTRAS

Estas son las tablas principales que contienen las entidades de negocio.

### 2.1 `Departments` - Departamentos

**Propósito:** Organiza a los empleados en departamentos.

**Estructura:**
```sql
DepartmentId (PK, bigint, IDENTITY)        -- ID único
Code (nvarchar(20), NOT NULL, UNIQUE)      -- Código único del departamento
Name (nvarchar(100), NOT NULL)             -- Nombre del departamento
HodEmployeeId (FK, bigint, NULL, UNIQUE)   -- Jefe del departamento
LocationId (FK, bigint, NULL)              -- Ubicación principal
CreatedAt (datetime2, NOT NULL)            -- Fecha de creación
UpdatedAt (datetime2, NULL)                 -- Última actualización
```

**Características:**
- ✅ `Code` es único (índice único)
- ✅ Un departamento puede tener UN jefe (HodEmployeeId)
- ✅ Un departamento puede estar en UNA ubicación
- ✅ Un empleado puede ser jefe de VARIOS departamentos

**Relaciones:**
- `HodEmployeeId` → `Employees.EmployeeId` (Restrict)
- `LocationId` → `Locations.LocationId`
- Un departamento tiene MUCHOS empleados → `Employees.DepartmentId`
- Un departamento tiene MUCHOS tickets → `Tickets.DepartmentId`

**Ejemplo:**
```csharp
var dept = new Department 
{
    Code = "IT",
    Name = "Tecnología de la Información",
    HodEmployeeId = 5,
    LocationId = 2
};
```

---

### 2.2 `Locations` - Ubicaciones

**Propósito:** Representa ubicaciones físicas o remotas donde pueden estar activos o empleados.

**Estructura:**
```sql
LocationId (PK, bigint, IDENTITY)          -- ID único
Name (nvarchar(100), NOT NULL)             -- Nombre de la ubicación
LocationTypeId (FK, smallint, NOT NULL)     -- Tipo de ubicación
Address (nvarchar(300), NULL)              -- Dirección completa
City (nvarchar(100), NULL)                 -- Ciudad
Country (nvarchar(100), NULL)              -- País
EmployeeId (FK, bigint, NULL)              -- Empleado asociado (si es domicilio)
CreatedAt (datetime2, NOT NULL)             -- Fecha de creación
```

**Relaciones:**
- `LocationTypeId` → `LocationTypes.LocationTypeId` (Cascade)
- `EmployeeId` → `Employees.EmployeeId` (para ubicaciones tipo "Domicilio")
- Una ubicación tiene MUCHOS departamentos → `Departments.LocationId`
- Una ubicación tiene MUCHOS activos → `Assets.CurrentLocationId`
- Historial de movimientos → `AssetAssignmentHistories`

**Ejemplo:**
```csharp
var location = new Location
{
    Name = "Oficina Central - Piso 3",
    LocationTypeId = 1,  // "Oficina"
    Address = "Av. Principal 123",
    City = "Ciudad",
    Country = "País"
};
```

---

### 2.3 `Employees` - Empleados

**Propósito:** Almacena información de todos los empleados de la organización.

**Estructura:**
```sql
EmployeeId (PK, bigint, IDENTITY)          -- ID único
EmployeeCode (nvarchar(20), NOT NULL, UNIQUE) -- Código único del empleado
FirstName (nvarchar(80), NOT NULL)         -- Nombre
LastName (nvarchar(80), NOT NULL)           -- Apellido
Email (nvarchar(200), NOT NULL, UNIQUE)     -- Email único
Phone (nvarchar(30), NULL)                  -- Teléfono
DepartmentId (FK, bigint, NULL)            -- Departamento
SupervisorId (FK, bigint, NULL)            -- Supervisor (autoreferencia)
StatusId (FK, smallint, NOT NULL)          -- Estado de empleo
ModalityId (FK, smallint, NOT NULL)        -- Modalidad de trabajo
IsItAdmin (bit, NOT NULL, DEFAULT 0)        -- Es administrador IT
HiredDate (datetime2, NULL)                 -- Fecha de contratación
TerminatedDate (datetime2, NULL)            -- Fecha de terminación
CreatedAt (datetime2, NOT NULL)             -- Fecha de creación
UpdatedAt (datetime2, NULL)                 -- Última actualización
```

**Características Importantes:**
- ✅ `EmployeeCode` es único (formato: "EMP-{EmployeeId}")
- ✅ `Email` es único
- ✅ `SupervisorId` es una **autoreferencia** (un empleado puede ser supervisor de otros)
- ✅ Un empleado puede ser jefe de departamento (`DepartmentsAsHead`)
- ✅ Un empleado puede tener un supervisor (`Supervisor`)
- ✅ Un empleado puede tener subordinados (`Subordinates`)

**Relaciones Principales:**
- `DepartmentId` → `Departments.DepartmentId`
- `SupervisorId` → `Employees.EmployeeId` (autoreferencia)
- `StatusId` → `EmploymentStatuses.StatusId` (Cascade)
- `ModalityId` → `Modalities.ModalityId` (Cascade)

**Relaciones Múltiples:**
- Un empleado puede crear MUCHOS tickets → `Tickets.CreatedByEmployeeId`
- Un empleado puede tener MUCHOS tickets asignados → `Tickets.AssignedToEmployeeId`
- Un empleado puede tener MUCHOS activos asignados → `Assets.CurrentAssignedEmployeeId`
- Historial de asignaciones (desde) → `AssetAssignmentHistories.FromEmployeeId`
- Historial de asignaciones (hacia) → `AssetAssignmentHistories.ToEmployeeId`
- Historial de asignaciones (asignado por) → `AssetAssignmentHistories.AssignedByEmployeeId`
- Historial de cambios de estado → `AssetStatusHistories.ChangedByEmployeeId`
- Mantenimientos reportados → `AssetMaintenanceRecords.ReportedByEmployeeId`
- Mantenimientos como técnico → `AssetMaintenanceRecords.TechnicianEmployeeId`
- Órdenes de compra creadas → `PurchaseOrders.CreatedByEmployeeId`
- Órdenes de compra aprobadas → `PurchaseOrders.ApprovedByEmployeeId`
- Aprobaciones de tickets → `TicketApprovals.ApproverEmployeeId`
- Comentarios en tickets → `TicketComments.AuthorEmployeeId`
- Licencias asignadas → `SoftwareLicenses.AssignedEmployeeId`

**Ejemplo:**
```csharp
var employee = new Employee
{
    EmployeeCode = "EMP-001",
    FirstName = "Juan",
    LastName = "Pérez",
    Email = "juan.perez@empresa.com",
    Phone = "+1234567890",
    DepartmentId = 1,
    SupervisorId = 5,
    StatusId = 1,      // "Activo"
    ModalityId = 2,    // "Oficina"
    IsItAdmin = false,
    HiredDate = DateTime.Now.AddYears(-2)
};
```

---

### 2.4 `Assets` - Activos

**Propósito:** Almacena información de todos los activos corporativos (equipos, hardware).

**Estructura:**
```sql
AssetId (PK, bigint, IDENTITY)              -- ID único
AssetTag (nvarchar(50), NOT NULL, UNIQUE)   -- Código único del activo
SerialNumber (nvarchar(100), NULL)          -- Número de serie
AssetTypeId (FK, smallint, NOT NULL)        -- Tipo de activo
Manufacturer (nvarchar(100), NULL)          -- Fabricante
Model (nvarchar(100), NULL)                 -- Modelo
PurchaseDate (datetime2, NULL)              -- Fecha de compra
PurchasePrice (decimal(12,2), NULL)         -- Precio de compra
CurrentStatusId (FK, smallint, NOT NULL)    -- Estado actual
CurrentLocationId (FK, bigint, NULL)        -- Ubicación actual
CurrentAssignedEmployeeId (FK, bigint, NULL) -- Empleado asignado actualmente
WarrantyExpiration (datetime2, NULL)        -- Expiración de garantía
Notes (nvarchar(max), NULL)                 -- Notas adicionales
ImageUrl (nvarchar(max), NULL)              -- URL de imagen (Google Cloud Storage)
CreatedAt (datetime2, NOT NULL)             -- Fecha de creación
UpdatedAt (datetime2, NULL)                 -- Última actualización
```

**Características:**
- ✅ `AssetTag` es único (formato: "AST-{AssetId}")
- ✅ Un activo tiene UN estado actual (`CurrentStatusId`)
- ✅ Un activo puede estar en UNA ubicación (`CurrentLocationId`)
- ✅ Un activo puede estar asignado a UN empleado (`CurrentAssignedEmployeeId`)
- ✅ Relación 1:1 con `HardwareSpecs` (especificaciones técnicas)

**Relaciones:**
- `AssetTypeId` → `AssetTypes.AssetTypeId` (Cascade)
- `CurrentStatusId` → `AssetStatuses.StatusId` (Cascade)
- `CurrentLocationId` → `Locations.LocationId`
- `CurrentAssignedEmployeeId` → `Employees.EmployeeId`
- Un activo tiene UNA especificación de hardware → `HardwareSpecs.AssetId` (1:1)
- Un activo tiene MUCHOS historiales de asignación → `AssetAssignmentHistories`
- Un activo tiene MUCHOS historiales de estado → `AssetStatusHistories`
- Un activo puede tener MUCHOS tickets relacionados → `Tickets.RelatedAssetId`
- Un activo puede tener MUCHAS licencias asignadas → `SoftwareLicenses.AssignedAssetId`
- Un activo puede tener MUCHOS mantenimientos → `AssetMaintenanceRecords`

**Ejemplo:**
```csharp
var asset = new Asset
{
    AssetTag = "AST-001",
    SerialNumber = "SN123456789",
    AssetTypeId = 2,  // "Laptop"
    Manufacturer = "Dell",
    Model = "Latitude 5520",
    PurchaseDate = DateTime.Now.AddMonths(-6),
    PurchasePrice = 1200.00m,
    CurrentStatusId = 2,  // "Asignado"
    CurrentLocationId = 1,
    CurrentAssignedEmployeeId = 3,
    WarrantyExpiration = DateTime.Now.AddYears(2)
};
```

---

### 2.5 `HardwareSpecs` - Especificaciones de Hardware

**Propósito:** Almacena las especificaciones técnicas detalladas de un activo (relación 1:1 con Assets).

**Estructura:**
```sql
AssetId (PK/FK, bigint, NOT NULL)          -- ID del activo (es también PK)
CpuModel (nvarchar(150), NULL)              -- Modelo del CPU
CpuCores (smallint, NULL)                   -- Número de núcleos
RamGb (smallint, NULL)                      -- RAM en GB
StorageGb (int, NULL)                       -- Almacenamiento en GB
GpuModel (nvarchar(150), NULL)              -- Modelo de GPU
GpuVramMb (int, NULL)                       -- VRAM en MB
Os (nvarchar(100), NULL)                    -- Sistema operativo
MacAddress (nvarchar(50), NULL)             -- Dirección MAC
CreatedAt (datetime2, NOT NULL)             -- Fecha de creación
```

**Características:**
- ✅ Relación **1:1** con `Assets`
- ✅ `AssetId` es tanto Primary Key como Foreign Key
- ✅ Si se elimina un Asset, se elimina automáticamente su HardwareSpec (Cascade)

**Relación:**
- `AssetId` → `Assets.AssetId` (Cascade, 1:1)

**Ejemplo:**
```csharp
var hardwareSpec = new HardwareSpec
{
    AssetId = 1,  // Mismo ID que el Asset
    CpuModel = "Intel Core i7-11800H",
    CpuCores = 8,
    RamGb = 16,
    StorageGb = 512,
    GpuModel = "NVIDIA RTX 3060",
    GpuVramMb = 6144,
    Os = "Windows 11 Pro",
    MacAddress = "00:1B:44:11:3A:B7"
};
```

---

## 📜 3. TABLAS DE HISTORIAL/AUDITORÍA

Estas tablas registran todos los cambios y movimientos para mantener un historial completo y trazabilidad.

### 3.1 `AssetAssignmentHistories` - Historial de Asignaciones

**Propósito:** Registra TODOS los movimientos y asignaciones de activos (asignación, devolución, transferencia).

**Estructura:**
```sql
AssetAssignmentId (PK, bigint, IDENTITY)    -- ID único
AssetId (FK, bigint, NOT NULL)              -- Activo movido
FromEmployeeId (FK, bigint, NULL)           -- Empleado origen (NULL si no había)
ToEmployeeId (FK, bigint, NULL)             -- Empleado destino (NULL si devolución)
FromLocationId (FK, bigint, NULL)           -- Ubicación origen
ToLocationId (FK, bigint, NULL)            -- Ubicación destino
AssignedByEmployeeId (FK, bigint, NULL)     -- Empleado que realizó la asignación
EventType (nvarchar(30), NOT NULL)          -- "Assigned", "Returned", "Transferred"
ConditionOnReturn (nvarchar(200), NULL)     -- Condición al retorno
Notes (nvarchar(max), NULL)                 -- Notas adicionales
MovedAt (datetime2, NOT NULL)               -- Fecha del movimiento
```

**Tipos de Eventos (`EventType`):**
- `"Assigned"` - Asignación a empleado
- `"Returned"` - Devolución de activo
- `"Transferred"` - Transferencia entre empleados/ubicaciones
- `"Moved"` - Movimiento de ubicación

**Características:**
- ✅ Registra TODOS los movimientos (no solo los actuales)
- ✅ Permite rastrear el historial completo de un activo
- ✅ Puede tener NULLs en campos origen/destino según el tipo de evento

**Relaciones:**
- `AssetId` → `Assets.AssetId` (Cascade)
- `FromEmployeeId` → `Employees.EmployeeId` (Restrict)
- `ToEmployeeId` → `Employees.EmployeeId` (Restrict)
- `FromLocationId` → `Locations.LocationId` (Restrict)
- `ToLocationId` → `Locations.LocationId` (Restrict)
- `AssignedByEmployeeId` → `Employees.EmployeeId` (Restrict)

**Ejemplo:**
```csharp
// Asignación inicial
var assignment = new AssetAssignmentHistory
{
    AssetId = 1,
    ToEmployeeId = 3,
    ToLocationId = 2,
    AssignedByEmployeeId = 5,
    EventType = "Assigned",
    MovedAt = DateTime.UtcNow
};

// Devolución
var returnRecord = new AssetAssignmentHistory
{
    AssetId = 1,
    FromEmployeeId = 3,
    FromLocationId = 2,
    AssignedByEmployeeId = 5,
    EventType = "Returned",
    ConditionOnReturn = "Buen estado, sin daños",
    MovedAt = DateTime.UtcNow
};
```

---

### 3.2 `AssetStatusHistories` - Historial de Estados

**Propósito:** Registra TODOS los cambios de estado de un activo.

**Estructura:**
```sql
AssetStatusHistId (PK, bigint, IDENTITY)    -- ID único
AssetId (FK, bigint, NOT NULL)              -- Activo
OldStatusId (FK, smallint, NULL)             -- Estado anterior (NULL si es el primero)
NewStatusId (FK, smallint, NOT NULL)        -- Estado nuevo
ChangedByEmployeeId (FK, bigint, NULL)       -- Empleado que cambió el estado
ChangedAt (datetime2, NOT NULL)             -- Fecha del cambio
Reason (nvarchar(300), NULL)                 -- Razón del cambio
```

**Características:**
- ✅ Registra cada cambio de estado
- ✅ `OldStatusId` puede ser NULL (primer estado)
- ✅ Permite auditoría completa de cambios

**Relaciones:**
- `AssetId` → `Assets.AssetId` (Cascade)
- `OldStatusId` → `AssetStatuses.StatusId` (Restrict)
- `NewStatusId` → `AssetStatuses.StatusId` (Restrict)
- `ChangedByEmployeeId` → `Employees.EmployeeId`

**Ejemplo:**
```csharp
var statusChange = new AssetStatusHistory
{
    AssetId = 1,
    OldStatusId = 2,  // "Asignado"
    NewStatusId = 3,  // "En Reparación"
    ChangedByEmployeeId = 5,
    ChangedAt = DateTime.UtcNow,
    Reason = "Pantalla dañada, requiere reparación"
};
```

---

## 💼 4. TABLAS TRANSACCIONALES

Estas tablas gestionan procesos de negocio y operaciones.

### 4.1 `Tickets` - Tickets de Soporte

**Propósito:** Sistema completo de gestión de tickets (solicitudes e incidencias).

**Estructura:**
```sql
TicketId (PK, bigint, IDENTITY)              -- ID único
TicketCode (nvarchar(30), NOT NULL, UNIQUE)  -- Código único del ticket
TicketTypeId (FK, smallint, NOT NULL)        -- Tipo: "Solicitud" o "Incidencia"
CreatedByEmployeeId (FK, bigint, NOT NULL)   -- Empleado que crea el ticket
AssignedToEmployeeId (FK, bigint, NULL)      -- Empleado asignado para resolver
DepartmentId (FK, bigint, NULL)             -- Departamento relacionado
RelatedAssetId (FK, bigint, NULL)            -- Activo relacionado (si aplica)
Subject (nvarchar(200), NOT NULL)            -- Asunto
Description (nvarchar(max), NOT NULL)         -- Descripción detallada
Priority (smallint, NOT NULL, DEFAULT 3)     -- 1=Crítica, 2=Alta, 3=Media, 4=Baja
StatusId (FK, smallint, NOT NULL)            -- Estado actual
CreatedAt (datetime2, NOT NULL)               -- Fecha de creación
ResolvedAt (datetime2, NULL)                 -- Fecha de resolución
SlaDueAt (datetime2, NULL)                   -- Fecha límite SLA
EstimatedCost (decimal(12,2), NULL)           -- Costo estimado
```

**Características:**
- ✅ `TicketCode` es único
- ✅ Sistema de prioridades (1-4)
- ✅ SLA tracking (`SlaDueAt`)
- ✅ Relacionado con activos, empleados y departamentos

**Relaciones:**
- `TicketTypeId` → `TicketTypes.TypeId` (Cascade)
- `CreatedByEmployeeId` → `Employees.EmployeeId` (Restrict)
- `AssignedToEmployeeId` → `Employees.EmployeeId` (Restrict)
- `DepartmentId` → `Departments.DepartmentId`
- `RelatedAssetId` → `Assets.AssetId`
- `StatusId` → `TicketStatuses.StatusId` (Cascade)
- Un ticket tiene MUCHAS aprobaciones → `TicketApprovals`
- Un ticket tiene MUCHOS comentarios → `TicketComments`
- Un ticket puede tener MUCHOS mantenimientos → `AssetMaintenanceRecords`

**Ejemplo:**
```csharp
var ticket = new Ticket
{
    TicketCode = "TKT-2024-001",
    TicketTypeId = 2,  // "Incidencia"
    CreatedByEmployeeId = 3,
    AssignedToEmployeeId = 5,  // IT Support
    RelatedAssetId = 1,
    Subject = "Laptop no enciende",
    Description = "La laptop Dell Latitude no enciende después de actualización",
    Priority = 2,  // Alta
    StatusId = 1,  // "Pendiente"
    SlaDueAt = DateTime.Now.AddHours(4)
};
```

---

### 4.2 `TicketApprovals` - Aprobaciones de Tickets

**Propósito:** Sistema de aprobaciones para tickets que requieren autorización.

**Estructura:**
```sql
TicketApprovalId (PK, bigint, IDENTITY)       -- ID único
TicketId (FK, bigint, NOT NULL)              -- Ticket a aprobar
ApproverEmployeeId (FK, bigint, NOT NULL)    -- Empleado que aprueba
ApproverRole (nvarchar(80), NULL)            -- Rol del aprobador
Sequence (smallint, NOT NULL, DEFAULT 1)     -- Orden de aprobación
Decision (nvarchar(20), NOT NULL, DEFAULT 'PENDING') -- 'PENDING', 'APPROVED', 'REJECTED'
DecisionDate (datetime2, NULL)                -- Fecha de decisión
Comments (nvarchar(max), NULL)               -- Comentarios
CreatedAt (datetime2, NOT NULL)               -- Fecha de creación
```

**Características:**
- ✅ Sistema de aprobaciones secuenciales (`Sequence`)
- ✅ Estados: PENDING, APPROVED, REJECTED
- ✅ Permite múltiples niveles de aprobación

**Relaciones:**
- `TicketId` → `Tickets.TicketId` (Cascade)
- `ApproverEmployeeId` → `Employees.EmployeeId` (Cascade)

---

### 4.3 `TicketComments` - Comentarios de Tickets

**Propósito:** Comentarios y actualizaciones en tickets.

**Estructura:**
```sql
CommentId (PK, bigint, IDENTITY)             -- ID único
TicketId (FK, bigint, NOT NULL)              -- Ticket
AuthorEmployeeId (FK, bigint, NOT NULL)      -- Autor del comentario
Comment (nvarchar(max), NOT NULL)            -- Contenido del comentario
Internal (bit, NOT NULL, DEFAULT 0)          -- Si es interno (no visible para usuario)
CreatedAt (datetime2, NOT NULL)               -- Fecha de creación
```

**Características:**
- ✅ Comentarios públicos e internos (`Internal`)
- ✅ Historial completo de conversaciones

**Relaciones:**
- `TicketId` → `Tickets.TicketId` (Cascade)
- `AuthorEmployeeId` → `Employees.EmployeeId` (Cascade)

---

### 4.4 `SoftwareLicenses` - Licencias de Software

**Propósito:** Gestiona licencias de software corporativas.

**Estructura:**
```sql
LicenseId (PK, bigint, IDENTITY)              -- ID único
ProductName (nvarchar(150), NOT NULL)         -- Nombre del producto
LicenseKey (nvarchar(255), NULL)              -- Clave de licencia
LicenseType (nvarchar(80), NULL)              -- Tipo: "Perpetua", "Anual", etc.
Seats (int, NULL)                             -- Número de asientos/dispositivos
PurchasedFrom (nvarchar(150), NULL)           -- Proveedor
PurchaseDate (datetime2, NULL)                -- Fecha de compra
ExpirationDate (datetime2, NULL)               -- Fecha de expiración
AssignedAssetId (FK, bigint, NULL)            -- Activo asignado
AssignedEmployeeId (FK, bigint, NULL)         -- Empleado asignado
Notes (nvarchar(max), NULL)                   -- Notas
```

**Relaciones:**
- `AssignedAssetId` → `Assets.AssetId`
- `AssignedEmployeeId` → `Employees.EmployeeId`
- Una licencia puede tener MUCHAS asignaciones → `LicenseAssignments`

---

### 4.5 `LicenseAssignments` - Asignaciones de Licencias

**Propósito:** Registra asignaciones específicas de licencias a activos (para licencias con múltiples asientos).

**Estructura:**
```sql
LicenseAssignmentId (PK, bigint, IDENTITY)    -- ID único
LicenseId (FK, bigint, NOT NULL)              -- Licencia
AssetId (FK, bigint, NOT NULL)                 -- Activo asignado
AssignedDate (datetime2, NOT NULL)             -- Fecha de asignación
SeatsUsed (int, NULL)                          -- Asientos utilizados
```

**Relaciones:**
- `LicenseId` → `SoftwareLicenses.LicenseId` (Cascade)
- `AssetId` → `Assets.AssetId` (Cascade)

---

### 4.6 `Vendors` - Proveedores

**Propósito:** Almacena información de proveedores.

**Estructura:**
```sql
VendorId (PK, bigint, IDENTITY)              -- ID único
Name (nvarchar(150), NOT NULL)                -- Nombre del proveedor
Contact (nvarchar(200), NULL)                 -- Información de contacto
CreatedAt (datetime2, NOT NULL)               -- Fecha de creación
```

**Relaciones:**
- Un proveedor tiene MUCHAS órdenes de compra → `PurchaseOrders`
- Un proveedor tiene MUCHOS mantenimientos → `AssetMaintenanceRecords`

---

### 4.7 `PurchaseOrders` - Órdenes de Compra

**Propósito:** Gestiona órdenes de compra de activos o servicios.

**Estructura:**
```sql
PoId (PK, bigint, IDENTITY)                   -- ID único
PoNumber (nvarchar(50), NOT NULL, UNIQUE)     -- Número único de orden
VendorId (FK, bigint, NOT NULL)               -- Proveedor
Amount (decimal(12,2), NOT NULL)               -- Monto total
CreatedByEmployeeId (FK, bigint, NOT NULL)     -- Empleado que crea
ApprovedByEmployeeId (FK, bigint, NULL)        -- Empleado que aprueba
ApprovedAt (datetime2, NULL)                   -- Fecha de aprobación
CreatedAt (datetime2, NOT NULL)                -- Fecha de creación
```

**Características:**
- ✅ `PoNumber` es único
- ✅ Sistema de aprobación (puede requerir aprobación)

**Relaciones:**
- `VendorId` → `Vendors.VendorId` (Cascade)
- `CreatedByEmployeeId` → `Employees.EmployeeId` (Restrict)
- `ApprovedByEmployeeId` → `Employees.EmployeeId` (Restrict)

---

### 4.8 `AssetMaintenanceRecords` - Registros de Mantenimiento

**Propósito:** Registra mantenimientos y reparaciones de activos.

**Estructura:**
```sql
MaintenanceId (PK, bigint, IDENTITY)          -- ID único
AssetId (FK, bigint, NOT NULL)                 -- Activo mantenido
ReportedByEmployeeId (FK, bigint, NOT NULL)    -- Empleado que reporta
VendorId (FK, bigint, NULL)                    -- Proveedor de mantenimiento
TechnicianEmployeeId (FK, bigint, NULL)        -- Técnico interno
StartDate (datetime2, NULL)                    -- Fecha de inicio
EndDate (datetime2, NULL)                      -- Fecha de finalización
Cost (decimal(12,2), NULL)                     -- Costo del mantenimiento
TicketId (FK, bigint, NULL)                    -- Ticket relacionado
Notes (nvarchar(max), NULL)                    -- Notas
CreatedAt (datetime2, NOT NULL)                 -- Fecha de creación
```

**Relaciones:**
- `AssetId` → `Assets.AssetId` (Cascade)
- `ReportedByEmployeeId` → `Employees.EmployeeId` (Restrict)
- `VendorId` → `Vendors.VendorId`
- `TechnicianEmployeeId` → `Employees.EmployeeId` (Restrict)
- `TicketId` → `Tickets.TicketId`

---

## 🔗 Relaciones entre Tablas

### Jerarquía de Empleados
```
Employee (Supervisor)
    └── Employee (Subordinado)
        └── Employee (Subordinado)
```

### Estructura Organizacional
```
Location
    └── Department (HodEmployeeId → Employee)
        └── Employee (DepartmentId → Department)
```

### Gestión de Activos
```
Asset (CurrentStatusId → AssetStatus)
    ├── HardwareSpec (1:1)
    ├── CurrentLocationId → Location
    ├── CurrentAssignedEmployeeId → Employee
    ├── AssetAssignmentHistories (historial completo)
    └── AssetStatusHistories (historial de estados)
```

### Sistema de Tickets
```
Ticket
    ├── TicketApprovals (aprobaciones)
    ├── TicketComments (comentarios)
    └── AssetMaintenanceRecords (mantenimientos relacionados)
```

---

## 🔐 Índices Únicos

La base de datos tiene los siguientes índices únicos para garantizar integridad:

1. ✅ `Employees.EmployeeCode` - Código único de empleado
2. ✅ `Employees.Email` - Email único
3. ✅ `Assets.AssetTag` - Código único de activo
4. ✅ `Tickets.TicketCode` - Código único de ticket
5. ✅ `PurchaseOrders.PoNumber` - Número único de orden
6. ✅ `Departments.Code` - Código único de departamento

---

## 🎯 Flujos de Trabajo

### 1. Asignación de Activo
```
1. Crear registro en AssetAssignmentHistory (EventType = "Assigned")
2. Actualizar Asset.CurrentAssignedEmployeeId
3. Actualizar Asset.CurrentLocationId
4. Actualizar Asset.CurrentStatusId = 2 ("Asignado")
5. Crear registro en AssetStatusHistory
```

### 2. Devolución de Activo
```
1. Crear registro en AssetAssignmentHistory (EventType = "Returned")
2. Limpiar Asset.CurrentAssignedEmployeeId (NULL)
3. Actualizar Asset.CurrentStatusId = 1 ("Disponible")
4. Crear registro en AssetStatusHistory
```

### 3. Creación de Ticket
```
1. Generar TicketCode único
2. Crear Ticket con StatusId = 1 ("Pendiente")
3. Si requiere aprobación, crear TicketApprovals
4. Asignar a empleado (AssignedToEmployeeId)
```

### 4. Aprobación de Ticket
```
1. Actualizar TicketApproval.Decision = "APPROVED"
2. Actualizar TicketApproval.DecisionDate
3. Si es última aprobación, actualizar Ticket.StatusId
```

---

## 💡 Ejemplos de Uso

### Crear un Empleado

```csharp
var employee = new Employee
{
    EmployeeCode = "EMP-001",
    FirstName = "María",
    LastName = "González",
    Email = "maria.gonzalez@empresa.com",
    Phone = "+1234567890",
    DepartmentId = 1,
    SupervisorId = 5,
    StatusId = 1,      // Activo
    ModalityId = 3,     // Híbrido
    IsItAdmin = false,
    HiredDate = DateTime.Now,
    CreatedAt = DateTime.UtcNow
};

context.Employees.Add(employee);
context.SaveChanges();
```

### Crear un Activo con Especificaciones

```csharp
// 1. Crear el activo
var asset = new Asset
{
    AssetTag = "AST-001",
    SerialNumber = "SN123456789",
    AssetTypeId = 2,  // Laptop
    Manufacturer = "Dell",
    Model = "Latitude 5520",
    PurchaseDate = DateTime.Now.AddMonths(-6),
    PurchasePrice = 1200.00m,
    CurrentStatusId = 1,  // Disponible
    CurrentLocationId = 1,
    CreatedAt = DateTime.UtcNow
};

context.Assets.Add(asset);
context.SaveChanges();

// 2. Agregar especificaciones técnicas (1:1)
var hardwareSpec = new HardwareSpec
{
    AssetId = asset.AssetId,
    CpuModel = "Intel Core i7-11800H",
    CpuCores = 8,
    RamGb = 16,
    StorageGb = 512,
    GpuModel = "NVIDIA RTX 3060",
    GpuVramMb = 6144,
    Os = "Windows 11 Pro",
    MacAddress = "00:1B:44:11:3A:B7",
    CreatedAt = DateTime.UtcNow
};

context.HardwareSpecs.Add(hardwareSpec);
context.SaveChanges();
```

### Asignar un Activo a un Empleado

```csharp
// 1. Crear registro en historial
var assignment = new AssetAssignmentHistory
{
    AssetId = 1,
    ToEmployeeId = 3,
    ToLocationId = 2,
    AssignedByEmployeeId = 5,
    EventType = "Assigned",
    Notes = "Asignación inicial",
    MovedAt = DateTime.UtcNow
};

context.AssetAssignmentHistories.Add(assignment);

// 2. Actualizar el activo
var asset = context.Assets.Find(1);
asset.CurrentAssignedEmployeeId = 3;
asset.CurrentLocationId = 2;
asset.CurrentStatusId = 2;  // Asignado
asset.UpdatedAt = DateTime.UtcNow;

// 3. Registrar cambio de estado
var statusChange = new AssetStatusHistory
{
    AssetId = 1,
    OldStatusId = 1,  // Disponible
    NewStatusId = 2,  // Asignado
    ChangedByEmployeeId = 5,
    ChangedAt = DateTime.UtcNow,
    Reason = "Asignación a empleado"
};

context.AssetStatusHistories.Add(statusChange);
context.SaveChanges();
```

### Consultas Útiles

```csharp
// Obtener todos los actives de un empleado
var employeeAssets = context.Assets
    .Where(a => a.CurrentAssignedEmployeeId == employeeId)
    .Include(a => a.AssetType)
    .Include(a => a.CurrentStatus)
    .Include(a => a.CurrentLocation)
    .Include(a => a.HardwareSpec)
    .ToList();

// Obtener historial completo de un activo
var assetHistory = context.AssetAssignmentHistories
    .Where(h => h.AssetId == assetId)
    .Include(h => h.FromEmployee)
    .Include(h => h.ToEmployee)
    .Include(h => h.FromLocation)
    .Include(h => h.ToLocation)
    .Include(h => h.AssignedByEmployee)
    .OrderByDescending(h => h.MovedAt)
    .ToList();

// Obtener tickets pendientes de un empleado
var pendingTickets = context.Tickets
    .Where(t => t.AssignedToEmployeeId == employeeId && 
                t.StatusId == 1)  // Pendiente
    .Include(t => t.TicketType)
    .Include(t => t.Status)
    .Include(t => t.RelatedAsset)
    .OrderBy(t => t.Priority)
    .ThenBy(t => t.SlaDueAt)
    .ToList();
```

---

## 📝 Notas Importantes

1. **Trazabilidad Completa:** Todas las acciones importantes se registran en tablas de historial
2. **Integridad Referencial:** Uso de Foreign Keys con comportamientos apropiados (Cascade, Restrict)
3. **Normalización:** Estructura normalizada para evitar redundancia
4. **Auditoría:** Campos `CreatedAt` y `UpdatedAt` en tablas principales
5. **Soft Deletes:** Algunas tablas usan estados en lugar de eliminar registros
6. **Escalabilidad:** Uso de `bigint` para IDs principales permite millones de registros

---

**Última actualización:** 2024-12-31  
**Versión de Base de Datos:** 3.0  
**Última migración:** 20251231004914_MigrateToDatabase3
