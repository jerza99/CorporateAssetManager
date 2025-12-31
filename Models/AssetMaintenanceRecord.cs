using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class AssetMaintenanceRecord
    {
        [Key]
        public long MaintenanceId { get; set; }

        public long AssetId { get; set; }

        public long ReportedByEmployeeId { get; set; }

        public long? VendorId { get; set; }

        public long? TechnicianEmployeeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Costo")]
        public decimal? Cost { get; set; }

        public long? TicketId { get; set; }

        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [ForeignKey("ReportedByEmployeeId")]
        public virtual Employee? ReportedByEmployee { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor? Vendor { get; set; }

        [ForeignKey("TechnicianEmployeeId")]
        public virtual Employee? TechnicianEmployee { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }
    }
}
