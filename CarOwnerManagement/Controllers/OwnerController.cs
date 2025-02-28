using CarOwnerManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement.Controllers
{
    [ApiController]
    [Route("api/owner")]
    public class OwnerController(CarManagementDbContext dbContext) : Controller
    {
        private readonly CarManagementDbContext _dbContext = dbContext;

        [HttpGet("get")]
        [Produces("application/json")]
        [ProducesResponseType<List<ResponseOwner>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOwnersAsync()
        {
            var owners = await _dbContext.Owners
                .Select(o => new ResponseOwner(o.Name, o.Cars.Count))
                .ToListAsync();

            return Ok(owners);
        }
    }
}
