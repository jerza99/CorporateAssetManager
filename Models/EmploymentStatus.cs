using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class EmploymentStatus
    {
        [Key]
        public short StatusId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
