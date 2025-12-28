using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nombre Completo")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Display(Name = "Teléfono")]
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Display(Name = "Edad")]
        [Range(18, 100, ErrorMessage = "La edad debe estar entre 18 y 100 años")]
        public int? Age { get; set; }

        [Display(Name = "Departamento")]
        [StringLength(100)]
        public string? Department { get; set; }

        [Display(Name = "Cargo")]
        [StringLength(100)]
        public string? JobTitle { get; set; }
    }
}

