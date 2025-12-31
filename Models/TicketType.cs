using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class TicketType
    {
        [Key]
        public short TypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
