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
        public async Task<IActionResult> GetOwnersAsync()
        {
            var owners = await _dbContext.Owners
                .Include(nameof(dbContext.Cars))
                .Select(o => new ResponseOwner
                {
                    Name = o.Name,
                    CarCount = o.Cars.Count
                })
                .ToListAsync();

            return Ok(owners);
        }
    }
}
