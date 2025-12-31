using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class Ticket
    {
        [Key]
        public long TicketId { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Código de Ticket")]
        public string TicketCode { get; set; } = string.Empty;

        public short TicketTypeId { get; set; }

        public long CreatedByEmployeeId { get; set; }

        public long? AssignedToEmployeeId { get; set; }

        public long? DepartmentId { get; set; }

        public long? RelatedAssetId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Asunto")]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Prioridad")]
        public short Priority { get; set; } = 3; // 1=Crítica, 2=Alta, 3=Media, 4=Baja

        public short StatusId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de Resolución")]
        public DateTime? ResolvedAt { get; set; }

        [Display(Name = "Fecha Límite SLA")]
        public DateTime? SlaDueAt { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Costo Estimado")]
        public decimal? EstimatedCost { get; set; }

        // Navigation
        [ForeignKey("TicketTypeId")]
        public virtual TicketType? TicketType { get; set; }

        [ForeignKey("CreatedByEmployeeId")]
        public virtual Employee? CreatedByEmployee { get; set; }

        [ForeignKey("AssignedToEmployeeId")]
        public virtual Employee? AssignedToEmployee { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [ForeignKey("RelatedAssetId")]
        public virtual Asset? RelatedAsset { get; set; }

        [ForeignKey("StatusId")]
        public virtual TicketStatus? Status { get; set; }

        // Relaciones
        public virtual ICollection<TicketApproval> TicketApprovals { get; set; } = new List<TicketApproval>();
        public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
        public virtual ICollection<AssetMaintenanceRecord> MaintenanceRecords { get; set; } = new List<AssetMaintenanceRecord>();
    }
}
