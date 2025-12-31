using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class Vendor
    {
        [Key]
        public long VendorId { get; set; }

        [Required]
        [MaxLength(150)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Contacto")]
        public string? Contact { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public virtual ICollection<AssetMaintenanceRecord> MaintenanceRecords { get; set; } = new List<AssetMaintenanceRecord>();
    }
}
