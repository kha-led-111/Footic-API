using footic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace footic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueStandingsController : ControllerBase
    {
        private readonly LeagueStandingService _leaguestandingservice;
        public LeagueStandingsController(LeagueStandingService leaguestandingservice) { 
        
         _leaguestandingservice = leaguestandingservice;
        
        }
        [HttpGet("standings")]
       //get standing
       //api/leauestandings/standings
        public async Task<IActionResult> GetStandings()
        {
            var reasult = await _leaguestandingservice.GetStandingAsync();
            return Ok(reasult);
        }
        [HttpGet("heighst")]
        //api/leauestandings/heighst
        public async Task<IActionResult> GetHighst()
        {
            var heighst = await _leaguestandingservice.Get_Highst_Async();
            return Ok(heighst);
        }

        [HttpGet("leader")]
        //api/leauestandings/leader
        public async Task<IActionResult> Getleader()
        {
            var leader = await _leaguestandingservice.GetLeagueLeaderAsync();
            return Ok(leader);
        }

        [HttpGet("progress")]
        ////api/leauestandings/progress
        public async Task<IActionResult> progress()
        {
            var progress = await _leaguestandingservice.CalculateLeagueProgressAsync();
            return Ok(progress);
        }


    }
}
