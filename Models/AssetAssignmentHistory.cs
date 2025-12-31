using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class AssetAssignmentHistory
    {
        [Key]
        public long AssetAssignmentId { get; set; }

        public long AssetId { get; set; }

        public long? FromEmployeeId { get; set; }

        public long? ToEmployeeId { get; set; }

        public long? FromLocationId { get; set; }

        public long? ToLocationId { get; set; }

        public long? AssignedByEmployeeId { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Tipo de Evento")]
        public string EventType { get; set; } = string.Empty; // "Assigned", "Returned", "Transferred", etc.

        [MaxLength(200)]
        [Display(Name = "Condición al Retorno")]
        public string? ConditionOnReturn { get; set; }

        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        public DateTime MovedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [ForeignKey("FromEmployeeId")]
        public virtual Employee? FromEmployee { get; set; }

        [ForeignKey("ToEmployeeId")]
        public virtual Employee? ToEmployee { get; set; }

        [ForeignKey("FromLocationId")]
        public virtual Location? FromLocation { get; set; }

        [ForeignKey("ToLocationId")]
        public virtual Location? ToLocation { get; set; }

        [ForeignKey("AssignedByEmployeeId")]
        public virtual Employee? AssignedByEmployee { get; set; }
    }
}
