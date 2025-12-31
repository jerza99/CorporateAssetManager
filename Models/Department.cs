using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class Department
    {
        [Key]
        public long DepartmentId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public long? HodEmployeeId { get; set; }

        public long? LocationId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("HodEmployeeId")]
        public virtual Employee? HeadOfDepartment { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
