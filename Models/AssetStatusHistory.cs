using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class AssetStatusHistory
    {
        [Key]
        public long AssetStatusHistId { get; set; }

        public long AssetId { get; set; }

        public short? OldStatusId { get; set; }

        public short NewStatusId { get; set; }

        public long? ChangedByEmployeeId { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(300)]
        [Display(Name = "Razón del Cambio")]
        public string? Reason { get; set; }

        // Navigation
        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [ForeignKey("OldStatusId")]
        public virtual AssetStatusLookup? OldStatus { get; set; }

        [ForeignKey("NewStatusId")]
        public virtual AssetStatusLookup? NewStatus { get; set; }

        [ForeignKey("ChangedByEmployeeId")]
        public virtual Employee? ChangedByEmployee { get; set; }
    }
}
