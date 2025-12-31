using Microsoft.EntityFrameworkCore;
using CorporateAssetManager.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace CorporateAssetManager.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager){
            // Verificar si la base de datos ha sido inicializada
            if (context.Database.GetPendingMigrations().Any()){
                context.Database.Migrate();
            }

            // Verificar si ahi datos en la base de datos
            if (context.Employees.Any() || context.Assets.Any())
            {
                return;
            }

            //EJECUTAR LOS SEEDS PARA LLENAR LA BASE DE DATOS CON DATOS DE PRUEBA
            SeedLookupTables(context);
            SeedMasterData(context);
            SeedEmployees(context);
            SeedAssets(context);
            SeedSoftwareLicenses(context);
            SeedTickets(context);
            
            // Crear roles y usuarios con roles asignados
            await SeedRoles(roleManager);
            await SeedUsersAndRoles(context, userManager);
        }

        public static void SeedLookupTables(ApplicationDbContext context){
            // Crear modalidades de trabajo
            if (!context.Modalities.Any())
            {
                context.Modalities.AddRange(
                    new Modality { Name = "Remoto", Description = "Trabajo completamente remoto" },
                    new Modality { Name = "Oficina", Description = "Trabajo en oficina física" },
                    new Modality { Name = "Híbrido", Description = "Combinación de remoto y oficina" }
                );
                context.SaveChanges();
            }

            // Crear estados de empleo
            if (!context.EmploymentStatuses.Any())
            {
                context.EmploymentStatuses.AddRange(
                    new EmploymentStatus { Name = "Activo" },
                    new EmploymentStatus { Name = "Inactivo" },
                    new EmploymentStatus { Name = "Licencia" },
                    new EmploymentStatus { Name = "Suspendido" },
                    new EmploymentStatus { Name = "Terminado" }
                );
                context.SaveChanges();
            }

            // Crear tipos de activos
            if (!context.AssetTypes.Any())
            {
                context.AssetTypes.AddRange(
                    new AssetType { Name = "Workstation" },
                    new AssetType { Name = "Laptop" },
                    new AssetType { Name = "GPU Externa" },
                    new AssetType { Name = "Tableta" },
                    new AssetType { Name = "Monitor" },
                    new AssetType { Name = "Servidor" },
                    new AssetType { Name = "Impresora" },
                    new AssetType { Name = "Scanner" },
                    new AssetType { Name = "Proyector" },
                    new AssetType { Name = "Router" },
                    new AssetType { Name = "Switch" },
                    new AssetType { Name = "Firewall" },
                    new AssetType { Name = "Cortacircuitos" }
                );
                context.SaveChanges();
            }

            // Crear estados de activos
            if (!context.AssetStatuses.Any())
            {
                context.AssetStatuses.AddRange(
                    new AssetStatusLookup { Name = "Disponible" },
                    new AssetStatusLookup { Name = "Asignado" },
                    new AssetStatusLookup { Name = "En Reparación" },
                    new AssetStatusLookup { Name = "Dañado" },
                    new AssetStatusLookup { Name = "Dado de Baja" }
                );
                context.SaveChanges();
            }

            // Crear tipos de ubicaciones
            if (!context.LocationTypes.Any())
            {
                context.LocationTypes.AddRange(
                    new LocationType { Name = "Oficina" },
                    new LocationType { Name = "Domicilio" },
                    new LocationType { Name = "Co-working" }
                );
                context.SaveChanges();
            }

            // Crear tipos de tickets
            if (!context.TicketTypes.Any())
            {
                context.TicketTypes.AddRange(
                    new TicketType { Name = "Solicitud" },
                    new TicketType { Name = "Incidencia" }
                );
                context.SaveChanges();
            }

            // Crear estados de tickets
            if (!context.TicketStatuses.Any())
            {
                context.TicketStatuses.AddRange(
                     new TicketStatus { Name = "Pendiente" },
                    new TicketStatus { Name = "En Revisión" },
                    new TicketStatus { Name = "Aprobado" },
                    new TicketStatus { Name = "Rechazado" },
                    new TicketStatus { Name = "En Proceso" },
                    new TicketStatus { Name = "Resuelto" },
                    new TicketStatus { Name = "Cerrado" }
                );
                context.SaveChanges();
            }
        }
    
        public static void SeedMasterData(ApplicationDbContext context){
            // Crear departamentos
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Code = "IT", Name = "Tecnología de la Información", CreatedAt = DateTime.UtcNow },
                    new Department { Code = "HR", Name = "Recursos Humanos", CreatedAt = DateTime.UtcNow },
                    new Department { Code = "FIN", Name = "Finanzas", CreatedAt = DateTime.UtcNow },
                    new Department { Code = "SALES", Name = "Ventas", CreatedAt = DateTime.UtcNow },
                    new Department { Code = "OPS", Name = "Operaciones", CreatedAt = DateTime.UtcNow }
                );
                context.SaveChanges();
            }

            // Crear ubicaciones
            if (!context.Locations.Any())
            {
                var locationTypeId = context.LocationTypes.FirstOrDefault(lt => lt.Name == "Oficina")?.LocationTypeId ?? 1;
                var faker = new Faker<Location>("es")
                    .RuleFor(l => l.Name, f => $"Oficina {f.Address.City()}")
                    .RuleFor(l => l.LocationTypeId, locationTypeId)
                    .RuleFor(l => l.Address, f => f.Address.FullAddress())
                    .RuleFor(l => l.City, f => f.Address.City())
                    .RuleFor(l => l.Country, f => f.Address.Country())
                    .RuleFor(l => l.CreatedAt, DateTime.UtcNow);

                var locations = faker.Generate(6);
                    context.Locations.AddRange(locations);
                    context.SaveChanges();
            }

            // Crear Vendors
            if (!context.Vendors.Any())
            {
                context.Vendors.AddRange(
                    new Vendor { Name = "Dell Technologies", Contact = "ventas@dell.com", CreatedAt = DateTime.UtcNow },
                    new Vendor { Name = "HP Inc.", Contact = "contacto@hp.com", CreatedAt = DateTime.UtcNow },
                    new Vendor { Name = "Microsoft Corporation", Contact = "licensing@microsoft.com", CreatedAt = DateTime.UtcNow },
                    new Vendor { Name = "Lenovo", Contact = "info@lenovo.com", CreatedAt = DateTime.UtcNow }
                );
                context.SaveChanges();
            }
        }
    
        public static void SeedEmployees(ApplicationDbContext context){
            // Crear empleados
            if (!context.Employees.Any()){
                var departments = context.Departments.ToList();
                var activeStatus = context.EmploymentStatuses.FirstOrDefault(s => s.Name == "Activo");
                var modalities = context.Modalities.ToList();

                if (departments.Any() && activeStatus != null && modalities.Any())
                {
                    int employeeCount = 1;
                    var faker = new Faker<Employee>("es")
                        .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                        .RuleFor(e => e.LastName, f => f.Name.LastName())
                        .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName, "empresa.com"))
                        .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                        .RuleFor(e => e.DepartmentId, f => f.PickRandom(departments).DepartmentId)
                        .RuleFor(e => e.StatusId, activeStatus.StatusId)
                        .RuleFor(e => e.ModalityId, f => f.PickRandom(modalities).ModalityId)
                        .RuleFor(e => e.IsItAdmin, false)
                        .RuleFor(e => e.HiredDate, f => f.Date.Between(DateTime.Now.AddYears(-3), DateTime.Now))
                        .RuleFor(e => e.CreatedAt, DateTime.UtcNow)
                        .RuleFor(e => e.EmployeeCode, f => $"EMP-{employeeCount++:D3}");
                        
                    var employees = faker.Generate(45);

                    // Guardar primero para obtener los EmployeeId
                    context.Employees.AddRange(employees);
                    context.SaveChanges();

                    // Ahora asignar supervisores (los primeros 5 no tienen supervisor, del 6 en adelante sí)
                    for (int i = 5; i < employees.Count; i++)
                    {
                        employees[i].SupervisorId = employees[i % 5].EmployeeId;
                    }
                    context.SaveChanges();
                }
                    
            }
        }

        private static void SeedAssets(ApplicationDbContext context)
        {
            if (!context.Assets.Any())
            {
                var assetTypes = context.AssetTypes.ToList();
                var assetStatuses = context.AssetStatuses.ToList();
                var locations = context.Locations.ToList();
                var employees = context.Employees.ToList();

                if (assetTypes.Any() && assetStatuses.Any() && locations.Any())
                {
                    int assetCounter = 1;
                    var manufacturers = new[] { "Dell", "HP", "Lenovo", "Apple", "ASUS" };
                    var models = new[] { "Latitude 5520", "EliteBook 850", "ThinkPad X1", "MacBook Pro", "ZenBook 14" };

                    var faker = new Faker<Asset>("es")
                        .RuleFor(a => a.AssetTag, f => $"AST-{assetCounter++:D3}")
                        .RuleFor(a => a.SerialNumber, f => $"SN{f.Random.AlphaNumeric(9).ToUpper()}")
                        .RuleFor(a => a.AssetTypeId, f => f.PickRandom(assetTypes).AssetTypeId)
                        .RuleFor(a => a.Manufacturer, f => f.PickRandom(manufacturers))
                        .RuleFor(a => a.Model, f => f.PickRandom(models))
                        .RuleFor(a => a.PurchaseDate, f => f.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now))
                        .RuleFor(a => a.PurchasePrice, f => f.Finance.Amount(500, 3000, 2))
                        .RuleFor(a => a.CurrentStatusId, f => f.PickRandom(assetStatuses).StatusId)
                        .RuleFor(a => a.CurrentLocationId, f => f.PickRandom(locations).LocationId)
                        .RuleFor(a => a.WarrantyExpiration, (f, a) => a.PurchaseDate?.AddYears(3))
                        .RuleFor(a => a.CreatedAt, DateTime.UtcNow);

                    var assets = faker.Generate(28);

                    // Asignar algunos activos a empleados (si el estado es "Asignado")
                    var assignedStatus = assetStatuses.FirstOrDefault(s => s.Name == "Asignado");
                    if (assignedStatus != null && employees.Any())
                    {
                        foreach (var asset in assets.Where(a => a.CurrentStatusId == assignedStatus.StatusId).Take(15))
                        {
                            asset.CurrentAssignedEmployeeId = employees[Random.Shared.Next(employees.Count)].EmployeeId;
                        }
                    }

                    context.Assets.AddRange(assets);
                    context.SaveChanges();

                    // Crear HardwareSpecs para cada Asset (1:1)
                    SeedHardwareSpecs(context, assets);
                }
            }
        }

        private static void SeedHardwareSpecs(ApplicationDbContext context, List<Asset> assets)
        {
            var cpuModels = new[] { "Intel Core i7-11800H", "AMD Ryzen 7 5800H", "Intel Core i5-11400H", "Apple M1 Pro" };
            var gpuModels = new[] { "NVIDIA RTX 3060", "NVIDIA RTX 3070", "AMD Radeon RX 6600", "Intel Iris Xe" };

            var hardwareSpecs = new List<HardwareSpec>();
            foreach (var asset in assets)
            {
                var faker = new Faker<HardwareSpec>()
                    .RuleFor(hs => hs.AssetId, asset.AssetId)
                    .RuleFor(hs => hs.CpuModel, f => f.PickRandom(cpuModels))
                    .RuleFor(hs => hs.CpuCores, f => f.Random.Short(4, 8))
                    .RuleFor(hs => hs.RamGb, f => f.Random.Short(8, 32))
                    .RuleFor(hs => hs.StorageGb, f => f.Random.Int(256, 1024))
                    .RuleFor(hs => hs.GpuModel, f => f.PickRandom(gpuModels))
                    .RuleFor(hs => hs.GpuVramMb, f => f.Random.Int(2048, 8192))
                    .RuleFor(hs => hs.Os, f => f.PickRandom(new[] { "Windows 11 Pro", "Windows 10 Pro", "macOS Monterey" }))
                    .RuleFor(hs => hs.MacAddress, f => f.Internet.Mac())
                    .RuleFor(hs => hs.CreatedAt, DateTime.UtcNow);

                hardwareSpecs.Add(faker.Generate());
            }

            context.HardwareSpecs.AddRange(hardwareSpecs);
            context.SaveChanges();
        } 

        private static void SeedSoftwareLicenses(ApplicationDbContext context)
        {
            if (!context.SoftwareLicenses.Any())
            {
                var employees = context.Employees.ToList();
                var assets = context.Assets.ToList();

                var products = new[] { "Microsoft Office 365", "Adobe Creative Suite", "Visual Studio Professional", "AutoCAD", "SolidWorks" };
                var licenseTypes = new[] { "Perpetua", "Anual", "Mensual" };

                var faker = new Faker<SoftwareLicense>("es")
                    .RuleFor(sl => sl.ProductName, f => f.PickRandom(products))
                    .RuleFor(sl => sl.LicenseKey, f => $"{f.Random.AlphaNumeric(5).ToUpper()}-{f.Random.AlphaNumeric(5).ToUpper()}-{f.Random.AlphaNumeric(5).ToUpper()}")
                    .RuleFor(sl => sl.LicenseType, f => f.PickRandom(licenseTypes))
                    .RuleFor(sl => sl.Seats, f => f.Random.Int(1, 10))
                    .RuleFor(sl => sl.PurchaseDate, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
                    .RuleFor(sl => sl.ExpirationDate, (f, sl) => sl.LicenseType == "Anual" ? sl.PurchaseDate?.AddYears(1) : null)
                    .RuleFor(sl => sl.PurchasedFrom, f => f.PickRandom(new[] { "Microsoft", "Adobe", "Autodesk" }));

                var licenses = faker.Generate(12);

                // Asignar algunas licencias a empleados o activos
                if (employees.Any() && assets.Any())
                {
                    foreach (var license in licenses.Take(8))
                    {
                        if (Random.Shared.Next(2) == 0)
                        {
                            license.AssignedEmployeeId = employees[Random.Shared.Next(employees.Count)].EmployeeId;
                        }
                        else
                        {
                            license.AssignedAssetId = assets[Random.Shared.Next(assets.Count)].AssetId;
                        }
                    }
                }

                context.SoftwareLicenses.AddRange(licenses);
                context.SaveChanges();
            }
        }

        private static void SeedTickets(ApplicationDbContext context)
        {
            if (!context.Tickets.Any())
            {
                var employees = context.Employees.ToList();
                var assets = context.Assets.ToList();
                var departments = context.Departments.ToList();
                var ticketTypes = context.TicketTypes.ToList();
                var ticketStatuses = context.TicketStatuses.ToList();

                if (employees.Any() && ticketTypes.Any() && ticketStatuses.Any())
                {
                    int ticketCounter = 1;
                    var faker = new Faker<Ticket>("es")
                        .RuleFor(t => t.TicketCode, f => $"TKT-2025-{ticketCounter++:D3}")
                        .RuleFor(t => t.TicketTypeId, f => f.PickRandom(ticketTypes).TypeId)
                        .RuleFor(t => t.CreatedByEmployeeId, f => f.PickRandom(employees).EmployeeId)
                        .RuleFor(t => t.Subject, f => f.Lorem.Sentence(3, 6))
                        .RuleFor(t => t.Description, f => f.Lorem.Paragraph(2))
                        .RuleFor(t => t.Priority, f => f.Random.Short(1, 4))
                        .RuleFor(t => t.StatusId, f => f.PickRandom(ticketStatuses).StatusId)
                        .RuleFor(t => t.CreatedAt, f => f.Date.Between(DateTime.Now.AddMonths(-2), DateTime.Now))
                        .RuleFor(t => t.SlaDueAt, (f, t) => t.CreatedAt.AddHours(f.Random.Int(4, 48)));

                    var tickets = faker.Generate(8);

                    // Asignar algunos tickets a empleados y activos
                    if (employees.Any() && assets.Any())
                    {
                        foreach (var ticket in tickets.Take(5))
                        {
                            ticket.AssignedToEmployeeId = employees[Random.Shared.Next(employees.Count)].EmployeeId;
                            if (Random.Shared.Next(2) == 0)
                            {
                                ticket.RelatedAssetId = assets[Random.Shared.Next(assets.Count)].AssetId;
                            }
                        }
                    }

                    context.Tickets.AddRange(tickets);
                    context.SaveChanges();
                }
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Definir los roles del sistema
            string[] roles = { "Admin", "Supervisor", "HeadOfDepartment", "Employee" };

            foreach (var roleName in roles)
            {
                // Verificar si el rol ya existe
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName)
                    {
                        NormalizedName = roleName.ToUpperInvariant()
                    };
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task SeedUsersAndRoles(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            var employees = context.Employees
                .Include(e => e.Department)
                .ToList();

            if (!employees.Any())
            {
                return;
            }

            // Crear usuarios para los primeros 10 empleados (para pruebas)
            // En producción, los usuarios se crearían cuando se registren
            var employeesToCreateUsers = employees.Take(10).ToList();

            foreach (var employee in employeesToCreateUsers)
            {
                // Verificar si ya existe un usuario para este empleado
                var existingUser = await userManager.FindByEmailAsync(employee.Email);
                if (existingUser != null)
                {
                    continue;
                }

                // Crear el usuario
                var user = new ApplicationUser
                {
                    UserName = employee.Email,
                    Email = employee.Email,
                    EmailConfirmed = true, // Para desarrollo, confirmamos automáticamente
                    EmployeeId = employee.EmployeeId
                };

                // Crear usuario con contraseña por defecto (para desarrollo)
                var result = await userManager.CreateAsync(user, "Password123!");
                
                if (result.Succeeded)
                {
                    // Determinar qué roles asignar según las condiciones del empleado
                    var rolesToAssign = new List<string>();

                    // 1. Si es administrador IT → rol "Admin"
                    if (employee.IsItAdmin)
                    {
                        rolesToAssign.Add("Admin");
                    }

                    // 2. Si es jefe de departamento → rol "HeadOfDepartment"
                    var isHeadOfDepartment = context.Departments
                        .Any(d => d.HodEmployeeId == employee.EmployeeId);
                    if (isHeadOfDepartment)
                    {
                        rolesToAssign.Add("HeadOfDepartment");
                    }

                    // 3. Si tiene subordinados (es supervisor) → rol "Supervisor"
                    var hasSubordinates = context.Employees
                        .Any(e => e.SupervisorId == employee.EmployeeId);
                    if (hasSubordinates)
                    {
                        rolesToAssign.Add("Supervisor");
                    }

                    // 4. Si no tiene ningún rol específico → rol "Employee"
                    if (!rolesToAssign.Any())
                    {
                        rolesToAssign.Add("Employee");
                    }
                    else
                    {
                        // Si tiene roles específicos, también agregar "Employee" como rol base
                        rolesToAssign.Add("Employee");
                    }

                    // Asignar los roles al usuario
                    foreach (var role in rolesToAssign.Distinct())
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            // Asignar roles a empleados que ya tienen usuarios pero no roles
            // (para casos donde el usuario se creó antes de los seeds)
            var allUsers = userManager.Users
                .Where(u => u.EmployeeId != null)
                .ToList();

            foreach (var user in allUsers)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                if (!userRoles.Any())
                {
                    var employee = employees.FirstOrDefault(e => e.EmployeeId == user.EmployeeId);
                    if (employee != null)
                    {
                        var rolesToAssign = new List<string>();

                        if (employee.IsItAdmin)
                        {
                            rolesToAssign.Add("Admin");
                        }

                        var isHeadOfDepartment = context.Departments
                            .Any(d => d.HodEmployeeId == employee.EmployeeId);
                        if (isHeadOfDepartment)
                        {
                            rolesToAssign.Add("HeadOfDepartment");
                        }

                        var hasSubordinates = context.Employees
                            .Any(e => e.SupervisorId == employee.EmployeeId);
                        if (hasSubordinates)
                        {
                            rolesToAssign.Add("Supervisor");
                        }

                        if (!rolesToAssign.Any())
                        {
                            rolesToAssign.Add("Employee");
                        }
                        else
                        {
                            rolesToAssign.Add("Employee");
                        }

                        foreach (var role in rolesToAssign.Distinct())
                        {
                            if (!userRoles.Contains(role))
                            {
                                await userManager.AddToRoleAsync(user, role);
                            }
                        }
                    }
                }
            }
        }
    }
}
