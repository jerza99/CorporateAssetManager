using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class LocationType
    {
        [Key]
        public short LocationTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
