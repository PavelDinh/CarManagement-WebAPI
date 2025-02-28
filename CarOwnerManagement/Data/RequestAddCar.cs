using System.ComponentModel.DataAnnotations;

namespace CarOwnerManagement.Data
{
    public record RequestAddCar(
        string Name,
        string? Description,
        [EnumDataType(typeof(FuelType))] FuelType? FuelType,
        ICollection<int> OwnersIds);
}
