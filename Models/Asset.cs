using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class Asset
    {
        [Key]
        public long AssetId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Código de Activo")]
        public string AssetTag { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Número de Serie")]
        public string? SerialNumber { get; set; }

        public short AssetTypeId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Fabricante")]
        public string? Manufacturer { get; set; }

        [MaxLength(100)]
        [Display(Name = "Modelo")]
        public string? Model { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Compra")]
        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Precio de Compra")]
        public decimal? PurchasePrice { get; set; }

        public short CurrentStatusId { get; set; }

        public long? CurrentLocationId { get; set; }

        public long? CurrentAssignedEmployeeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiración de Garantía")]
        public DateTime? WarrantyExpiration { get; set; }

        [Display(Name = "Notas")]
        public string? Notes { get; set; }

        // Link para guardar imagen en google cloud storage
        [Display(Name = "Foto de equipo")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("AssetTypeId")]
        public virtual AssetType? AssetType { get; set; }

        [ForeignKey("CurrentStatusId")]
        public virtual AssetStatusLookup? CurrentStatus { get; set; }

        [ForeignKey("CurrentLocationId")]
        public virtual Location? CurrentLocation { get; set; }

        [ForeignKey("CurrentAssignedEmployeeId")]
        public virtual Employee? CurrentAssignedEmployee { get; set; }

        // Relaciones
        public virtual HardwareSpec? HardwareSpec { get; set; }
        public virtual ICollection<AssetAssignmentHistory> AssetAssignmentHistories { get; set; } = new List<AssetAssignmentHistory>();
        public virtual ICollection<AssetStatusHistory> AssetStatusHistories { get; set; } = new List<AssetStatusHistory>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<SoftwareLicense> SoftwareLicenses { get; set; } = new List<SoftwareLicense>();
        public virtual ICollection<LicenseAssignment> LicenseAssignments { get; set; } = new List<LicenseAssignment>();
        public virtual ICollection<AssetMaintenanceRecord> MaintenanceRecords { get; set; } = new List<AssetMaintenanceRecord>();

        // Propiedades calculadas para compatibilidad
        [NotMapped]
        [Display(Name = "Nombre de equipo")]
        public string Name 
        { 
            get => AssetTag; 
            set => AssetTag = value; 
        }

        // Propiedades de compatibilidad para migración gradual
        [NotMapped]
        public int Id => (int)AssetId;

        // SerialNumber ya existe como propiedad real, no necesita compatibilidad

        [NotMapped]
        public DateTime CreatedDate 
        { 
            get => PurchaseDate ?? DateTime.UtcNow; 
            set => PurchaseDate = value; 
        }

        [NotMapped]
        public decimal Cost 
        { 
            get => PurchasePrice ?? 0; 
            set => PurchasePrice = value; 
        }

        [NotMapped]
        public AssetStatus Status 
        { 
            get 
            {
                // Mapeo básico - necesitarás ajustar según tus valores de AssetStatusLookup
                return CurrentStatusId switch
                {
                    1 => AssetStatus.Available,
                    2 => AssetStatus.Assigned,
                    3 => AssetStatus.UnderMaintenance,
                    4 => AssetStatus.Broken,
                    5 => AssetStatus.Retired,
                    _ => AssetStatus.Available
                };
            }
            set 
            {
                // Mapeo básico - necesitarás ajustar según tus valores de AssetStatusLookup
                CurrentStatusId = value switch
                {
                    AssetStatus.Available => 1,
                    AssetStatus.Assigned => 2,
                    AssetStatus.UnderMaintenance => 3,
                    AssetStatus.Broken => 4,
                    AssetStatus.Retired => 5,
                    _ => 1
                };
            }
        }
    }

    // Enum legacy para compatibilidad
    public enum AssetStatus
    {
        Available,
        Assigned,
        UnderMaintenance,
        Broken,
        Retired
    }
}
