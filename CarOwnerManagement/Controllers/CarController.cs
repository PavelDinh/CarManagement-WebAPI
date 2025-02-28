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
    [Produces("application/json")]
    [ProducesResponseType<ResponseCreatedCar>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCar([FromBody] RequestAddCar car, CancellationToken token)
    {
        if (string.IsNullOrEmpty(car.Name))
        {
            return BadRequest("Car name is required");
        }

        try
        {
            var createdCar = await _carRepo.CreateCarAsync(car, token);
            if (createdCar != null)
            {
                var response = new ResponseCreatedCar(
                    createdCar.Id,
                    createdCar.Name,
                    createdCar.Description,
                    createdCar.FuelType,
                    [.. createdCar.Owners.Select(o => o.Id)]);

                return CreatedAtAction(nameof(GetCarDetails), new { id = response.Id }, response);
            }

            return BadRequest("Failed to create car");
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
            var recordsAffected = await _carRepo.UpdateCarAsync(car, token);
            return Ok($"Records affected: {recordsAffected}");
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
            var recordsAffected = await _carRepo.DeleteCarAsync(id, token);
            return Ok($"Records affected: {recordsAffected}");
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
    [Produces("application/json")]
    [ProducesResponseType<List<ResponseCreatedCar>>(StatusCodes.Status200OK)]
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
    [Produces("application/json")]
    [ProducesResponseType<ResponseCarDetails>(StatusCodes.Status200OK)]
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
