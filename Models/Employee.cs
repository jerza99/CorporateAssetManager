using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class Employee
    {
        public int Id { get; set; } // PK

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "El correo electrónico es obligatorio")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Departamento")]
        public string Department { get; set; }

        [Display(Name = "Cargo")]
        public string JobTitle { get; set; }

        public bool IsActive { get; set; } = true;

        //Relacion entre tablas
        public virtual ICollection<AssetAssignment>? Assignments { get; set; }
    }
}
