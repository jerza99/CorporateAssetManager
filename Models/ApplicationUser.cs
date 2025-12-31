using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Relación con Employee (fuente de verdad para datos del negocio)
        public long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        // Age se mantiene solo si es necesario para autenticación
        // Si no lo necesitas, puedes eliminarlo también
        [Display(Name = "Edad")]
        [Range(18, 100, ErrorMessage = "La edad debe estar entre 18 y 100 años")]
        public int? Age { get; set; }

        // Propiedades calculadas para acceso a datos del Employee relacionado
        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string FullName => Employee?.FullName ?? string.Empty;

        [NotMapped]
        [Display(Name = "Teléfono")]
        public string? Phone => Employee?.Phone;

        [NotMapped]
        [Display(Name = "Departamento")]
        public string? Department => Employee?.Department?.Name;

        [NotMapped]
        [Display(Name = "Cargo")]
        public string? JobTitle => Employee?.JobTitle;
    }
}

