using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class AssetAssignment
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        public virtual Asset? Asset { get; set; } 
        
        public int EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }  

        [Display(Name = "Fecha de Asignación")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Devolución")]
        public DateTime? ReturnDate { get; set; } 

        [Display(Name = "Comentarios")]
        public string? Comments { get; set; }
    }
}
