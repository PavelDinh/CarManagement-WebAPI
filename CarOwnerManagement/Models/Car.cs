using CarOwnerManagement.Data;
using System.ComponentModel.DataAnnotations;

namespace CarOwnerManagement.Models
{
    public record Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [EnumDataType(typeof(FuelType))]
        public FuelType? FuelType { get; set; }

        public ICollection<Owner> Owners { get; set; } = [];
    }
}
