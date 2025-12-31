using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class SoftwareLicense
    {
        [Key]
        public long LicenseId { get; set; }

        [Required]
        [MaxLength(150)]
        [Display(Name = "Nombre del Producto")]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(255)]
        [Display(Name = "Clave de Licencia")]
        public string? LicenseKey { get; set; }

        [MaxLength(80)]
        [Display(Name = "Tipo de Licencia")]
        public string? LicenseType { get; set; }

        [Display(Name = "Asientos")]
        public int? Seats { get; set; }

        [MaxLength(150)]
        [Display(Name = "Comprado de")]
        public string? PurchasedFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Compra")]
        public DateTime? PurchaseDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Expiración")]
        public DateTime? ExpirationDate { get; set; }

        public long? AssignedAssetId { get; set; }

        public long? AssignedEmployeeId { get; set; }

        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        // Navigation
        [ForeignKey("AssignedAssetId")]
        public virtual Asset? AssignedAsset { get; set; }

        [ForeignKey("AssignedEmployeeId")]
        public virtual Employee? AssignedEmployee { get; set; }

        // Relaciones
        public virtual ICollection<LicenseAssignment> LicenseAssignments { get; set; } = new List<LicenseAssignment>();
    }
}
