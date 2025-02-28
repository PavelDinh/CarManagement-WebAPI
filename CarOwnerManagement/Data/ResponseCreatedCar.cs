using System.ComponentModel.DataAnnotations;

namespace CarOwnerManagement.Data
{
    public record ResponseCreatedCar(
        int Id,
        string Name,
        string? Description,
        [EnumDataType(typeof(FuelType))]
        FuelType? FuelType,
        ICollection<int> OwnerIds);
}
