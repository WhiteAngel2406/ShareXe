using System.ComponentModel.DataAnnotations;

using ShareXe.Base.Attributes;
using ShareXe.Base.Entities;

namespace ShareXe.Modules.Hubs.Entities
{
    [Entity("hubs")]
    public class Hub : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }
    }
}
