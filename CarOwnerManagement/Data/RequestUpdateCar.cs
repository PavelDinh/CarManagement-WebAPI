using System.ComponentModel.DataAnnotations;

namespace CarOwnerManagement.Data
{
    public record RequestUpdateCar(
        int Id,
        string Name, 
        string? Description,
        [EnumDataType(typeof(FuelType))] FuelType? FuelType,
        ICollection<int> OwnersIds);
}
