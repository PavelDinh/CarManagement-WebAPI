using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarOwnerManagement.Data
{
    public class ResponseCarDetails
    {
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FuelType? FuelType { get; set; }
        
        public ICollection<string> OwnerNames { get; set; } = [];
    }
}
