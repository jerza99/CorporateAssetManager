using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class AssetStatusLookup
    {
        [Key]
        public short StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public virtual ICollection<AssetStatusHistory> AssetStatusHistories { get; set; } = new List<AssetStatusHistory>();
    }
}
