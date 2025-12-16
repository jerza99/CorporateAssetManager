using System.ComponentModel.DataAnnotations;

namespace CorporateAssetManager.Models
{
    public class Asset
    {
        public int Id { get; set; } // PK

        [Required]
        [Display(Name = "Nombre de equipo")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Numero de serie")]
        public string SerialNumber { get; set; }

        //Link para guardar imagen en google cloud storage
        [Display(Name = "Foto de equipo")]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de compra")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Costo")]
        public decimal Cost { get; set; }

        [Display(Name = "Estado Actual")]
        public AssetStatus Status { get; set; }

    }

    public enum AssetStatus
    {
        Available,
        Assigned,
        UnderMaintenance,
        Broken,
        Retired
    }
}
