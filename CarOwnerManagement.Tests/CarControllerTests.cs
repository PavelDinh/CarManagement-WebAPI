using CarOwnerManagement.Controllers;
using CarOwnerManagement.Data;
using CarOwnerManagement.Models;
using CarOwnerManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarOwnerManagement.Tests;
public class CarControllerTests
{
    private readonly Mock<ILogger<CarController>> _loggerMock;
    private readonly Mock<ICarRepository> _carRepoMock;
    private readonly CarController _controller;

    public CarControllerTests()
    {
        _loggerMock = new Mock<ILogger<CarController>>();
        _carRepoMock = new Mock<ICarRepository>();
        _controller = new CarController(_loggerMock.Object, _carRepoMock.Object);
    }

    [Fact]
    public async Task CreateCar_ReturnsBadRequest_WhenCarNameIsNull()
    {
        // Arrange
        var car = new RequestAddCar(string.Empty, null, null, []);

        // Act
        var result = await _controller.CreateCar(car, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Car name is required", badRequestResult.Value);
    }

    [Fact]
    public async Task CreateCar_ReturnsCreatedAtAction_WhenCarIsCreated()
    {
        // Arrange
        var car = new RequestAddCar("Test Car", null, null, []);
        var createdCar = new ResponseCreatedCar(1, "Test Car", null, null, []);
        var entityCar = new Car { Id = 1, Name = "Test Car" };
        _carRepoMock.Setup(repo => repo.CreateCarAsync(car, It.IsAny<CancellationToken>())).ReturnsAsync(entityCar);

        // Act
        var result = await _controller.CreateCar(car, CancellationToken.None);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetCarDetails), createdAtActionResult.ActionName);
        Assert.Equivalent(createdCar, createdAtActionResult.Value);
    }

    [Fact]
    public async Task UpdateCar_ReturnsOk_WhenCarIsUpdated()
    {
        // Arrange
        var car = new RequestUpdateCar(1, "UpdatedCar", null, null, []);

        // Act
        var result = await _controller.UpdateCar(car, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCarById_ReturnsOk_WhenCarIsDeleted()
    {
        // Arrange
        var carId = 1;

        // Act
        var result = await _controller.DeleteCarById(carId, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetCars_ReturnsBadRequest_WhenPageOrPageSizeIsInvalid()
    {
        // Act
        var result = await _controller.GetCars(null, 0, 10, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid page or pageSize", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCars_ReturnsOk_WithPaginatedCars()
    {
        // Arrange
        var cars = new List<ResponseCarSimple> { new(Name: "Test Car", null) };
        _carRepoMock.Setup(repo => repo.GetCarsPaginatedAsync(null, 1, 10, It.IsAny<CancellationToken>())).ReturnsAsync(cars);

        // Act
        var result = await _controller.GetCars(null, 1, 10, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(cars, okResult.Value);
    }

    [Fact]
    public async Task GetCarDetails_ReturnsOk_WithCarDetails()
    {
        // Arrange
        var carDetails = new ResponseCarDetails { Name = "Test Car" };
        _carRepoMock.Setup(repo => repo.GetCarDetailsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(carDetails);

        // Act
        var result = await _controller.GetCarDetails(1, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(carDetails, okResult.Value);
    }
}
