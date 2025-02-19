using CarOwnerManagement.Data;
using CarOwnerManagement.Models;
using CarOwnerManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement.Tests
{
    public class CarRepositoryTests
    {
        private static DbContextOptions<CarManagementDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<CarManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private static void SeedDatabase(CarManagementDbContext dbContext)
        {
            var owners = new List<Owner>
            {
                new() { Id = 1, Name = "Owner1" },
                new() { Id = 2, Name = "Owner2" }
            };

            var cars = new List<Car>
            {
                new() { Id = 1, Name = "Car1", Owners = [owners[0]] },
                new() { Id = 2, Name = "Car2", Owners = [owners[1]] }
            };

            dbContext.Owners.AddRange(owners);
            dbContext.Cars.AddRange(cars);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task CreateCarAsync_CreatesCarSuccessfully()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var dbContext = new CarManagementDbContext(options);
            SeedDatabase(dbContext);
            var repository = new CarRepository(dbContext);

            var request = new RequestAddCar
            {
                Name = "NewCar",
                Description = "NewCarDescription",
                FuelType = FuelType.Gasoline,
                OwnersIds = [1]
            };

            // Act
            var result = await repository.CreateCarAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NewCar", result.Name);
            Assert.Equal("NewCarDescription", result.Description);
            Assert.Equal(FuelType.Gasoline, result.FuelType);
            Assert.Single(result.Owners);
        }

        [Fact]
        public async Task GetCarsPaginatedAsync_ReturnsPaginatedCars()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var dbContext = new CarManagementDbContext(options);
            SeedDatabase(dbContext);
            var repository = new CarRepository(dbContext);

            // Act
            var result = await repository.GetCarsPaginatedAsync(null, 1, 1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Car1", result[0].Name);
        }

        [Fact]
        public async Task GetCarDetailsAsync_ReturnsCarDetails()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var dbContext = new CarManagementDbContext(options);
            SeedDatabase(dbContext);
            var repository = new CarRepository(dbContext);

            // Act
            var result = await repository.GetCarDetailsAsync(1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Car1", result.Name);
            Assert.Single(result.OwnerNames);
            Assert.Equal("Owner1", result.OwnerNames.First());
        }

        [Fact]
        public async Task UpdateCarAsync_UpdatesCarSuccessfully()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var dbContext = new CarManagementDbContext(options);
            SeedDatabase(dbContext);
            var repository = new CarRepository(dbContext);

            var request = new RequestUpdateCar
            {
                Id = 1,
                Name = "UpdatedCar",
                Description = "UpdatedDescription",
                FuelType = FuelType.Diesel,
                OwnersIds = [1]
            };

            // Act
            await repository.UpdateCarAsync(request, CancellationToken.None);

            // Assert
            var updatedCar = await dbContext.Cars.Include(c => c.Owners).FirstOrDefaultAsync(c => c.Id == 1);
            Assert.NotNull(updatedCar);
            Assert.Equal("UpdatedCar", updatedCar.Name);
            Assert.Equal("UpdatedDescription", updatedCar.Description);
            Assert.Equal(FuelType.Diesel, updatedCar.FuelType);
            Assert.Single(updatedCar.Owners);
            Assert.Equal(1, updatedCar.Owners.First().Id);
        }
    }
}