using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CorporateAssetManager.Models;

namespace CorporateAssetManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Lookup Tables
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<EmploymentStatus> EmploymentStatuses { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<AssetStatusLookup> AssetStatuses { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }

        // Master Tables
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<HardwareSpec> HardwareSpecs { get; set; }

        // History and Assignment Tables
        public DbSet<AssetAssignmentHistory> AssetAssignmentHistories { get; set; }
        public DbSet<AssetStatusHistory> AssetStatusHistories { get; set; }

        // Ticket System
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketApproval> TicketApprovals { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }

        // Software and Vendors
        public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }
        public DbSet<LicenseAssignment> LicenseAssignments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<AssetMaintenanceRecord> AssetMaintenanceRecords { get; set; }

        // Legacy table (mantener por compatibilidad temporal)
        public DbSet<AssetAssignment> AssetAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuraciones adicionales si son necesarias
            // Por ejemplo, índices únicos, valores por defecto, etc.

            // Índice único para EmployeeCode
            builder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique();

            // Índice único para AssetTag
            builder.Entity<Asset>()
                .HasIndex(a => a.AssetTag)
                .IsUnique();

            // Índice único para TicketCode
            builder.Entity<Ticket>()
                .HasIndex(t => t.TicketCode)
                .IsUnique();

            // Índice único para PoNumber
            builder.Entity<PurchaseOrder>()
                .HasIndex(po => po.PoNumber)
                .IsUnique();

            // Índice único para Department.Code
            builder.Entity<Department>()
                .HasIndex(d => d.Code)
                .IsUnique();

            // Índice único para Employee.Email
            builder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Relación 1:1 entre Asset y HardwareSpec
            builder.Entity<HardwareSpec>()
                .HasOne(hs => hs.Asset)
                .WithOne(a => a.HardwareSpec)
                .HasForeignKey<HardwareSpec>(hs => hs.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relaciones múltiples de AssetAssignmentHistory con Employee
            builder.Entity<AssetAssignmentHistory>()
                .HasOne(aah => aah.FromEmployee)
                .WithMany(e => e.AssetAssignmentHistoriesFrom)
                .HasForeignKey(aah => aah.FromEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssetAssignmentHistory>()
                .HasOne(aah => aah.ToEmployee)
                .WithMany(e => e.AssetAssignmentHistoriesTo)
                .HasForeignKey(aah => aah.ToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssetAssignmentHistory>()
                .HasOne(aah => aah.AssignedByEmployee)
                .WithMany(e => e.AssetAssignmentHistoriesAssignedBy)
                .HasForeignKey(aah => aah.AssignedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de AssetAssignmentHistory con Location
            builder.Entity<AssetAssignmentHistory>()
                .HasOne(aah => aah.FromLocation)
                .WithMany(l => l.AssetAssignmentHistoriesFrom)
                .HasForeignKey(aah => aah.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssetAssignmentHistory>()
                .HasOne(aah => aah.ToLocation)
                .WithMany(l => l.AssetAssignmentHistoriesTo)
                .HasForeignKey(aah => aah.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de AssetMaintenanceRecord con Employee
            builder.Entity<AssetMaintenanceRecord>()
                .HasOne(amr => amr.ReportedByEmployee)
                .WithMany(e => e.MaintenanceRecordsReported)
                .HasForeignKey(amr => amr.ReportedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssetMaintenanceRecord>()
                .HasOne(amr => amr.TechnicianEmployee)
                .WithMany(e => e.MaintenanceRecordsTechnician)
                .HasForeignKey(amr => amr.TechnicianEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de PurchaseOrder con Employee
            builder.Entity<PurchaseOrder>()
                .HasOne(po => po.CreatedByEmployee)
                .WithMany(e => e.PurchaseOrdersCreated)
                .HasForeignKey(po => po.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseOrder>()
                .HasOne(po => po.ApprovedByEmployee)
                .WithMany(e => e.PurchaseOrdersApproved)
                .HasForeignKey(po => po.ApprovedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de AssetStatusHistory con AssetStatusLookup
            builder.Entity<AssetStatusHistory>()
                .HasOne(ash => ash.OldStatus)
                .WithMany()
                .HasForeignKey(ash => ash.OldStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssetStatusHistory>()
                .HasOne(ash => ash.NewStatus)
                .WithMany()
                .HasForeignKey(ash => ash.NewStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de Department con Employee
            builder.Entity<Department>()
                .HasOne(d => d.HeadOfDepartment)
                .WithMany(e => e.DepartmentsAsHead)
                .HasForeignKey(d => d.HodEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones múltiples de Ticket con Employee
            builder.Entity<Ticket>()
                .HasOne(t => t.CreatedByEmployee)
                .WithMany(e => e.TicketsCreated)
                .HasForeignKey(t => t.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.AssignedToEmployee)
                .WithMany(e => e.TicketsAssigned)
                .HasForeignKey(t => t.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre ApplicationUser y Employee (1:1 opcional)
            builder.Entity<ApplicationUser>()
                .HasOne(au => au.Employee)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<ApplicationUser>(au => au.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull); // Si se elimina Employee, el usuario puede seguir existiendo
        }
    }
}
