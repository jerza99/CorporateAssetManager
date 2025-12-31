using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class Employee
    {
        [Key]
        public long EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Código de Empleado")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(30)]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        public long? DepartmentId { get; set; }

        public long? SupervisorId { get; set; }

        public short StatusId { get; set; }

        public short ModalityId { get; set; }

        [Display(Name = "Es Administrador IT")]
        public bool IsItAdmin { get; set; } = false;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Contratación")]
        public DateTime? HiredDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Terminación")]
        public DateTime? TerminatedDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual Employee? Supervisor { get; set; }

        [ForeignKey("StatusId")]
        public virtual EmploymentStatus? EmploymentStatus { get; set; }

        [ForeignKey("ModalityId")]
        public virtual Modality? Modality { get; set; }

        // Relaciones
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
        public virtual ICollection<Department> DepartmentsAsHead { get; set; } = new List<Department>();
        public virtual ICollection<Asset> AssetsAssigned { get; set; } = new List<Asset>();
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistoriesFrom { get; set; } = new List<AssetAssignmentHistory>();
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistoriesTo { get; set; } = new List<AssetAssignmentHistory>();
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistoriesAssignedBy { get; set; } = new List<AssetAssignmentHistory>();
        public virtual ICollection<Ticket> TicketsCreated { get; set; } = new List<Ticket>();
        public virtual ICollection<Ticket> TicketsAssigned { get; set; } = new List<Ticket>();
        public virtual ICollection<TicketApproval> TicketApprovals { get; set; } = new List<TicketApproval>();
        public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
        public virtual ICollection<AssetStatusHistory> AssetStatusHistories { get; set; } = new List<AssetStatusHistory>();
        public virtual ICollection<SoftwareLicense> SoftwareLicensesAssigned { get; set; } = new List<SoftwareLicense>();
        public virtual ICollection<AssetMaintenanceRecord> MaintenanceRecordsReported { get; set; } = new List<AssetMaintenanceRecord>();
        public virtual ICollection<AssetMaintenanceRecord> MaintenanceRecordsTechnician { get; set; } = new List<AssetMaintenanceRecord>();
        public virtual ICollection<PurchaseOrder> PurchaseOrdersCreated { get; set; } = new List<PurchaseOrder>();
        public virtual ICollection<PurchaseOrder> PurchaseOrdersApproved { get; set; } = new List<PurchaseOrder>();

        // Relación con ApplicationUser (Identity)
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // Propiedades calculadas para compatibilidad
        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {LastName}";

        // Propiedades de compatibilidad para migración gradual
        [NotMapped]
        public int Id => (int)EmployeeId;

        [NotMapped]
        public string PhoneNumber 
        { 
            get => Phone ?? string.Empty; 
            set => Phone = value; 
        }

        // Department ahora es una relación (DepartmentId + navegación Department)
        // El código existente debe migrarse a usar DepartmentId o Department.Name

        [NotMapped]
        public string? JobTitle { get; set; } // No está en BD3, se mantiene temporalmente

        [NotMapped]
        public bool IsActive 
        { 
            get => StatusId == 1; // Asumiendo que 1 = Activo
            set => StatusId = value ? (short)1 : (short)2; // Asumiendo que 2 = Inactivo
        }
    }
}
