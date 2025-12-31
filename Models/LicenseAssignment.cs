using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class LicenseAssignment
    {
        [Key]
        public long LicenseAssignmentId { get; set; }

        public long LicenseId { get; set; }

        public long AssetId { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Asientos Usados")]
        public int? SeatsUsed { get; set; }

        // Navigation
        [ForeignKey("LicenseId")]
        public virtual SoftwareLicense? License { get; set; }

        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }
    }
}
