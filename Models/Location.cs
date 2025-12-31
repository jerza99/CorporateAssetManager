using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class Location
    {
        [Key]
        public long LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public short LocationTypeId { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        public long? EmployeeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("LocationTypeId")]
        public virtual LocationType? LocationType { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistoriesFrom { get; set; } = new List<AssetAssignmentHistory>();
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistoriesTo { get; set; } = new List<AssetAssignmentHistory>();
    }
}
