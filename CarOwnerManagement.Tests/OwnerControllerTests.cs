using CarOwnerManagement.Controllers;
using CarOwnerManagement.Data;
using CarOwnerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement.Tests
{
    public class OwnerControllerTests
    {
        private readonly OwnerController _controller;
        private readonly CarManagementDbContext _dbContext;

        public OwnerControllerTests()
        {
            var options = new DbContextOptionsBuilder<CarManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new CarManagementDbContext(options);
            _controller = new OwnerController(_dbContext);

            // Seed the in-memory database with test data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var owners = new List<Owner>
            {
                new Owner { Name = "Owner1", Cars = new List<Car> { new Car { Name = "Car1" } } },
                new Owner { Name = "Owner2", Cars = new List<Car> { new Car { Name = "Car2" }, new Car { Name = "Car3" } } }
            };

            _dbContext.Owners.AddRange(owners);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetOwnersAsync_ReturnsOk_WithOwnersList()
        {
            // Act
            var result = await _controller.GetOwnersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOwners = Assert.IsType<List<ResponseOwner>>(okResult.Value);
            Assert.Equal(2, returnedOwners.Count);
        }
    }
}