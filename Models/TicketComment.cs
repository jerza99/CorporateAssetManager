using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class TicketComment
    {
        [Key]
        public long CommentId { get; set; }

        public long TicketId { get; set; }

        public long AuthorEmployeeId { get; set; }

        [Required]
        [Display(Name = "Comentario")]
        public string Comment { get; set; } = string.Empty;

        [Display(Name = "Es Interno")]
        public bool Internal { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey("AuthorEmployeeId")]
        public virtual Employee? AuthorEmployee { get; set; }
    }
}
