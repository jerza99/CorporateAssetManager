using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class AssetType
    {
        [Key]
        public short AssetTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
