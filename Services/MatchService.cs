using System.Text.RegularExpressions;
using footic.DTOs.Match;
using footic.EData;
using footic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;

namespace footic.Services
{
    public class MatchService
    {
        private readonly PlSimulationDbContext _context;
        public MatchService(PlSimulationDbContext context)
        {
            _context = context;
        }
        //return all matches
        public async Task<List<MatchDTO>> GetallMatches()
        {
            var matches = await _context.Matches.AsNoTracking().ToListAsync();
            var returnedmatches = matches.Select(Match => new MatchDTO
            {
                Id = Match.MatchId,
                HomeTeamID = Match.HomeTeamId,
                AwayTeamID = Match.AwayTeamId,
                HomeTeamScore = Match.Status == MatchState.Finished ? Match.HomeTeamScore : null,
                AwayTeamScore = Match.Status == MatchState.Finished ? Match.AwayTeamScore : null,
                MatchState = Match.Status,
                Matchdate = Match.MatchDate,
                Matchtime= Match.MatchTime,
                stadiumID = Match.StadiumId ?? 1,
            }).ToList();

            return returnedmatches;
        }

        public async Task<MatchDTO> GetMatchById(int matchid)
        {

            var match = await _context.Matches
           .Include(m => m.HomeTeam)
           .Include(m => m.AwayTeam)
           .Include(m => m.Stadium)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.MatchId == matchid);

            if (match == null) return null;

            bool showScore = match.Status == MatchState.Finished || match.Status == MatchState.Live || match.Status == MatchState.Upcoming || match.Status == MatchState.Postponed;

            return new MatchDTO
            {
                Id = match.MatchId,
                HomeTeamID = match.HomeTeamId,
                AwayTeamID = match.AwayTeamId,

                // إخفاء الأهداف لو المباراة لم تبدأ بعد
                HomeTeamScore = showScore ? match.HomeTeamScore : null,
                AwayTeamScore = showScore ? match.AwayTeamScore : null,

                MatchState = match.Status,
                Matchdate = match.MatchDate,
                Matchtime=match.MatchTime,
                stadiumID = match.StadiumId ?? 0,

            };


        }
    }
}
