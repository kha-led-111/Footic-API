using footic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace footic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _teamService;
        public TeamsController(TeamService teamService) { 
        
          _teamService = teamService;
        }


        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetTeamDetails(int id)
        {
            var result = await _teamService.GetTeamDetailsAsync(id);

            if (result == null)
            {
                return NotFound(new { Message = "Team not found" });
            }

            return Ok(result);
        }
    }
}
