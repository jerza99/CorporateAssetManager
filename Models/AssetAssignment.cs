using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    /// <summary>
    /// Tabla legacy - Se mantiene por compatibilidad temporal.
    /// Los nuevos registros deben usar AssetAssignmentHistory.
    /// </summary>
    public class AssetAssignment
    {
        public int Id { get; set; }

        public long AssetId { get; set; }

        public virtual Asset? Asset { get; set; } 
        
        public long EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }  

        [Display(Name = "Fecha de Asignación")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Devolución")]
        public DateTime? ReturnDate { get; set; } 

        [Display(Name = "Comentarios")]
        public string? Comments { get; set; }
    }
}
