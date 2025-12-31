using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class PurchaseOrder
    {
        [Key]
        public long PoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Número de Orden")]
        public string PoNumber { get; set; } = string.Empty;

        public long VendorId { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        public long CreatedByEmployeeId { get; set; }

        public long? ApprovedByEmployeeId { get; set; }

        [Display(Name = "Fecha de Aprobación")]
        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("VendorId")]
        public virtual Vendor? Vendor { get; set; }

        [ForeignKey("CreatedByEmployeeId")]
        public virtual Employee? CreatedByEmployee { get; set; }

        [ForeignKey("ApprovedByEmployeeId")]
        public virtual Employee? ApprovedByEmployee { get; set; }
    }
}
