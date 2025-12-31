using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class TicketApproval
    {
        [Key]
        public long TicketApprovalId { get; set; }

        public long TicketId { get; set; }

        public long ApproverEmployeeId { get; set; }

        [MaxLength(80)]
        [Display(Name = "Rol del Aprobador")]
        public string? ApproverRole { get; set; }

        [Display(Name = "Secuencia")]
        public short Sequence { get; set; } = 1;

        [Required]
        [MaxLength(20)]
        [Display(Name = "Decisión")]
        public string Decision { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED

        [Display(Name = "Fecha de Decisión")]
        public DateTime? DecisionDate { get; set; }

        [Display(Name = "Comentarios")]
        public string? Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("TicketId")]
        public virtual Ticket? Ticket { get; set; }

        [ForeignKey("ApproverEmployeeId")]
        public virtual Employee? ApproverEmployee { get; set; }
    }
}
