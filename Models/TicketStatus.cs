using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class TicketStatus
    {
        [Key]
        public short StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
