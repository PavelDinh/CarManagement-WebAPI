﻿using CarOwnerManagement.Data;
using CarOwnerManagement.Models;

namespace CarOwnerManagement.Repositories
{
    public interface ICarRepository
    {
        Task<Car> CreateCarAsync(RequestAddCar car, CancellationToken token);

        Task<List<ResponseCarSimple>> GetCarsPaginatedAsync(string? query, int page, int pageSize, CancellationToken token);

        Task<ResponseCarDetails?> GetCarDetailsAsync(int id, CancellationToken token);

        Task<int> UpdateCarAsync(RequestUpdateCar car, CancellationToken token);

        Task<int> DeleteCarAsync(int id, CancellationToken token);
    }
}
