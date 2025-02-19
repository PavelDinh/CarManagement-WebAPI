﻿using System.ComponentModel.DataAnnotations;

namespace CarOwnerManagement.Data
{
    public record RequestAddCar
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [EnumDataType(typeof(FuelType))]
        public FuelType? FuelType { get; set; }

        public ICollection<int> OwnersIds { get; set; } = [];
    }
}
