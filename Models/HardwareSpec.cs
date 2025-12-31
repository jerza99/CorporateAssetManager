using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateAssetManager.Models
{
    public class HardwareSpec
    {
        [Key]
        [ForeignKey("Asset")]
        public long AssetId { get; set; }

        [MaxLength(150)]
        [Display(Name = "Modelo CPU")]
        public string? CpuModel { get; set; }

        [Display(Name = "Núcleos CPU")]
        public short? CpuCores { get; set; }

        [Display(Name = "RAM (GB)")]
        public short? RamGb { get; set; }

        [Display(Name = "Almacenamiento (GB)")]
        public int? StorageGb { get; set; }

        [MaxLength(150)]
        [Display(Name = "Modelo GPU")]
        public string? GpuModel { get; set; }

        [Display(Name = "VRAM GPU (MB)")]
        public int? GpuVramMb { get; set; }

        [MaxLength(100)]
        [Display(Name = "Sistema Operativo")]
        public string? Os { get; set; }

        [MaxLength(50)]
        [Display(Name = "Dirección MAC")]
        public string? MacAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual Asset? Asset { get; set; }
    }
}
