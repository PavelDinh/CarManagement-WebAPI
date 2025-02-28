using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarOwnerManagement.Data
{
    public class ResponseCarDetails
    {
        public string Name { get; init; }

        public string? Description { get; init; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        public FuelType? FuelType { get; init; }
        
        public ICollection<string> OwnerNames { get; init; }
    }
}
