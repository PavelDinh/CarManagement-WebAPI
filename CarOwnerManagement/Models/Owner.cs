using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarOwnerManagement.Models
{
    public record Owner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        
        public ICollection<Car> Cars { get; set; } = [];
    }
}
