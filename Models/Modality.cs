using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class Modality
    {
        [Key]
        public short ModalityId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Navigation
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
