using CarOwnerManagement.Data;
using CarOwnerManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement.Repositories
{
    public class CarRepository(CarManagementDbContext dbContext) : ICarRepository
    {
        private readonly CarManagementDbContext _dbContext = dbContext;

        public async Task<Car> CreateCarAsync(RequestAddCar car, CancellationToken token)
        {
            if (car.OwnersIds == null)
            {
                throw new ArgumentNullException(nameof(car), "OwnerIds cannot be null!");
            }

            var owners = await _dbContext.Owners
                .Where(o => car.OwnersIds.Contains(o.Id))
                .ToListAsync(token);

            var newCar = new Car
            {
                Name = car.Name,
                Description = car.Description,
                FuelType = car.FuelType,
                Owners = owners,
            };

            await _dbContext.Cars.AddAsync(newCar, token);
            await _dbContext.SaveChangesAsync(token);

            return newCar;
        }

        public Task<List<ResponseCarSimple>> GetCarsPaginatedAsync(string? query, int page, int pageSize, CancellationToken token)
        {
            if (page < 1 || pageSize < 1)
            {
                return Task.FromResult<List<ResponseCarSimple>>([]);
            }

            var sqlQuery = _dbContext.Cars
                 .Select(c => new ResponseCarSimple
                 {
                     Name = c.Name,
                     Description = c.Description,
                 });

            if (!string.IsNullOrEmpty(query))
            {
                sqlQuery = sqlQuery.Where(c => c.Name.Contains(query) || (c.Description != null && c.Description.Contains(query)));
            }

            return sqlQuery
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .AsNoTracking()
                   .ToListAsync(token);
        }

        public Task<ResponseCarDetails?> GetCarDetailsAsync(int id, CancellationToken token)
        {
            return _dbContext.Cars
                .Where(c => c.Id == id)
                .Select(c => new ResponseCarDetails
                {
                    Name = c.Name,
                    Description = c.Description,
                    FuelType = c.FuelType,
                    OwnerNames = c.Owners.Select(o => o.Name).ToList(),
                }).FirstOrDefaultAsync(token);
        }

        public async Task UpdateCarAsync(RequestUpdateCar car, CancellationToken token)
        {
            if (car.OwnersIds == null)
            {
                throw new ArgumentNullException(nameof(car), "OwnersIds cannot be null!");
            }

            var carToUpdate = await _dbContext.Cars
                .Include(c => c.Owners)
                .FirstOrDefaultAsync(c => c.Id == car.Id, token) ?? throw new ArgumentNullException(nameof(car), "Car not found!");

            var owners = await _dbContext.Owners
                .Where(o => car.OwnersIds.Contains(o.Id))
                .ToListAsync(token);

            carToUpdate.Name = car.Name;
            carToUpdate.Description = car.Description;
            carToUpdate.FuelType = car.FuelType;
            carToUpdate.Owners = owners;

            await _dbContext.SaveChangesAsync(token);
        }

        public async Task DeleteCarAsync(int id, CancellationToken token)
        {
            var car = await _dbContext.Cars
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(token); // Cannot be tested with InMemoryDatabase
        }
    }
}
