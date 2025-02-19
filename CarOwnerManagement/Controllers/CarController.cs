using CarOwnerManagement.Data;
using CarOwnerManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement.Controllers;

[ApiController]
[Route("api/car")]
public class CarController(ILogger<CarController> logger, ICarRepository carRepo) : ControllerBase
{
    private readonly ILogger<CarController> _logger = logger;
    private readonly ICarRepository _carRepo = carRepo;

    /// <summary>
    /// Create a new car (including owners)
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateCar([FromBody] RequestAddCar car, CancellationToken token)
    {
        if (string.IsNullOrEmpty(car.Name))
        {
            return BadRequest("Car name is required");
        }

        try
        {
            var createdCar = await _carRepo.CreateCarAsync(car, token);
            return CreatedAtAction(nameof(GetCarDetails), new { id = createdCar.Id }, createdCar);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error");
            return StatusCode(StatusCodes.Status409Conflict, $"Database error: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex,"{message}", ex.Message);
            return BadRequest("Operation was canceled.");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Update an existing car (including owners)
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCar([FromBody] RequestUpdateCar car, CancellationToken token)
    {
        try
        {
            await _carRepo.UpdateCarAsync(car, token);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return StatusCode(StatusCodes.Status409Conflict, $"Database error: {ex.Message}");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteCarById(int id, CancellationToken token)
    {
        try
        {
            await _carRepo.DeleteCarAsync(id, token);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return StatusCode(StatusCodes.Status409Conflict, $"Database error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get a paginated list of cars with search support
    /// </summary>
    /// <param name="search">By Name or Description</param>
    /// <param name="page"></param>
    /// <param name="pageSize">d</param>
    /// <returns></returns>
    [HttpGet("get-paginated")]
    public async Task<IActionResult> GetCars([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken token = default)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid page or pageSize");
        }

        try
        {
            var cars = await _carRepo.GetCarsPaginatedAsync(search, page, pageSize, token);
            return Ok(cars);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest("Operation was canceled.");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest("Invalid search query");
        }
    }

    /// <summary>
    /// Get car details with owner names
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("get-details")]
    public async Task<IActionResult> GetCarDetails(int id, CancellationToken token)
    {
        try
        {
            var cars = await _carRepo.GetCarDetailsAsync(id, token);
            return Ok(cars);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest("Invalid search query");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            return BadRequest("Operation was canceled.");
        }
    }
}
