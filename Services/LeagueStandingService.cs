using footic.DTOs.League;
using footic.EData;
using footic.Models;
using Microsoft.EntityFrameworkCore;

namespace footic.Services
{
    public class LeagueStandingService
    {
        private readonly PlSimulationDbContext _context;
        public LeagueStandingService(PlSimulationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaueStandingDTO>> GetStandingAsync()
        {
            //bring teams
            var teams = await _context.Teams.Include(t=>t.TeamStat).AsNoTracking().ToListAsync();
            //finished matches
            var allFinishedMatches = await _context.Matches
              .Where(m => m.Status == MatchState.Finished)
              .OrderByDescending(m => m.MatchDate) // الترتيب من الأحدث للأقدم مهم جداً للـ Form
              .AsNoTracking()
              .ToListAsync();

            //create Dto 
            var standings = teams.Select(Team => new LeaueStandingDTO
            {
                TeamId = Team.TeamId,
                TeamName = Team.Tname,
                LogoUrl = (Team.Logo ?? ""),
                Played = (Team.TeamStat?.DrawNumber ?? 0) +
                         (Team.TeamStat?.WinsNumber ?? 0) +
                         (Team.TeamStat?.LoseNumber ?? 0),

                Won = Team.TeamStat?.WinsNumber ?? 0,
                Drawn = Team.TeamStat?.DrawNumber ?? 0,
                Lost = Team.TeamStat?.LoseNumber ?? 0,
                GoalsFor = Team.TeamStat?.GoalsFor ?? 0,
                GoalsAgainst = Team.TeamStat?.GoalsAgainst ?? 0,
                Points = Team.TeamStat?.Points ?? 0,
                LastFiveMatches = allFinishedMatches
                .Where(m => m.HomeTeamId == Team.TeamId || m.AwayTeamId == Team.TeamId)
                .Take(5)
                .Select(m => GetResultChar(Team.TeamId, m))
                 .ToList()

            }).ToList();
           
            return standings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenByDescending(s => s.GoalsFor)
                .ThenBy(s => s.TeamName)
                .ToList();
        }

        public async Task<LeagueHighstDTO> Get_Highst_Async()
        {
            var teams = await _context.Teams.Include(t => t.TeamStat).AsNoTracking().ToListAsync();
            //the current positions
            var currentStandings = teams
              .OrderByDescending(t => t.TeamStat?.Points ?? 0)
              .ThenByDescending(t => (t.TeamStat?.GoalsFor ?? 0) - (t.TeamStat?.GoalsAgainst ?? 0))
              .ToList();
            //get matches
            var matches = await _context.Matches
            .Where(m => m.Status==MatchState.Finished)
            .AsNoTracking()
            .ToListAsync();


            return new LeagueHighstDTO {
                TopScoring=GetTopScorer(teams),
                BestDefense=GetBestDefense(teams),
                TopClimber=GetTopClimber(teams),
                TopSlider=GetTopSlider(teams),
                WorstDefense=GetWorstDefense(teams),
                BestGD=GetBestGD(teams),
                TopHomeWinner=GetTopHomeWinner(teams,matches),
                TopAwayWinner=GetTopAwayWinner(teams,matches),
            };

        }

        public async Task<LeaueLeaderDTO> GetLeagueLeaderAsync()
        {
            var leaderTeam = await _context.Teams
          .Include(t => t.TeamStat)
          .Include(t => t.Players)
          .Where(t => t.TeamStat.Position == 1)
          .FirstOrDefaultAsync();

            if (leaderTeam == null) return null;

            var topScorerInTeam = leaderTeam.Players
            .OrderByDescending(p => p.PlayerStat.Goals)
            .FirstOrDefault();

            return new LeaueLeaderDTO
            {
                TeamName = leaderTeam.Tname,
                TeamLogo = leaderTeam.Logo,
                Points = leaderTeam.TeamStat.Points,

                // بيانات الهداف مع التعامل مع حالة عدم وجود لاعبين
                TopScorerName = topScorerInTeam?.Pname ?? "لا يوجد لاعبين",
                TopScorerGoals = topScorerInTeam?.PlayerStat.Goals ?? 0,
                TopScorerImage = topScorerInTeam?.Pimage ?? "default-player.png" // تأكد إن اسم الحقل Image في الموديل
            };


        }

        public async Task<int> CalculateLeagueProgressAsync(int leagueId=1)
        {
            // 1. نجيب إجمالي عدد الماتشات المفروض تتلعب في الدوري ده
            var totalMatches = 36;
                //await _context.Matches
                //.CountAsync(m => m.LeagueId==leagueId);

            if (totalMatches == 0) return 0;

            // 2. نجيب عدد الماتشات اللي خلصت فعلاً
            var finishedMatches = await _context.Matches
                .CountAsync(m => m.LeagueId == leagueId && m.Status == MatchState.Finished);

            // 3. النسبة المئوية للتقدم
            double progress = ((double)finishedMatches / totalMatches) * 100;

            return (int)Math.Round(progress);
        }

        private char GetResultChar(int teamId, Match match)
        {
            if (match.HomeTeamScore == match.AwayTeamScore) return 'D'; // تعادل

            // هل الفريق كان صاحب الأرض وكسب؟ أو كان الضيف وكسب؟
            bool isWinner = (match.HomeTeamId == teamId && match.HomeTeamScore > match.AwayTeamScore) ||
                           (match.AwayTeamId == teamId && match.AwayTeamScore > match.HomeTeamScore);

            return isWinner ? 'W' : 'L';
        }

        //calculate highst values
        #region helper functions
        //top scorer
        private HighstTeamDTO GetTopScorer(List<Team> teams)=>
            teams.OrderByDescending(t => t.TeamStat?.GoalsFor ?? 0)
             .Select(t => MapToHighlight(t, t.TeamStat?.GoalsFor ?? 0))
             .FirstOrDefault();
        //best defense
        private HighstTeamDTO GetBestDefense(List<Team> teams) =>
            teams.OrderBy(t => t.TeamStat?.GoalsAgainst ?? 0)
            .Select(t => MapToHighlight(t, t.TeamStat?.GoalsAgainst ?? 0)).FirstOrDefault();
        //worst defense
        private HighstTeamDTO GetWorstDefense(List<Team> teams) =>
        teams.OrderByDescending(t => t.TeamStat?.GoalsAgainst ?? 0)
             .Select(t => MapToHighlight(t, t.TeamStat?.GoalsAgainst ?? 0))
             .FirstOrDefault();
        //Best gaol difference
        private HighstTeamDTO GetBestGD(List<Team> teams) =>
        teams.OrderByDescending(t => (t.TeamStat?.GoalsFor ?? 0) - (t.TeamStat?.GoalsAgainst ?? 0))
             .Select(t => MapToHighlight(t, (t.TeamStat?.GoalsFor ?? 0) - (t.TeamStat?.GoalsAgainst ?? 0)))
             .FirstOrDefault();

        //Best home record
        private HighstTeamDTO GetTopHomeWinner(List<Team> teams, List<Match> matches)
        {
            var topTeamId = matches.Where(m => m.HomeTeamScore > m.AwayTeamScore)
                .GroupBy(m => m.HomeTeamId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .FirstOrDefault();

            if (topTeamId == null) return null;
            var team = teams.First(t => t.TeamId == topTeamId.Id);
            return MapToHighlight(team, topTeamId.Count);
        }
        //best away record
        private HighstTeamDTO GetTopAwayWinner(List<Team> teams, List<Match> matches)
        {
            var topTeamId = matches.Where(m => m.AwayTeamScore > m.HomeTeamScore)
                .GroupBy(m => m.AwayTeamId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Id = g.Key, Count = g.Count() })
                .FirstOrDefault();

            if (topTeamId == null) return null;
            var team = teams.First(t => t.TeamId == topTeamId.Id);
            return MapToHighlight(team, topTeamId.Count);
        }
        //bigst climb
        private HighstTeamDTO GetTopClimber(List<Team> teams)
        {
            // 1. يفضل عمل الحسبة دي بطريقة Defensive
            var climber = teams
                .Where(t => t.TeamStat != null && t.TeamStat.PreviousPosition > 0) // تأكد إن فيه إحصائيات ومركز سابق
                .Select(team => new
                {
                    Team = team,
                    // بنستخدم الـ Position والـ PreviousPosition بأمان
                    Jump = (team.TeamStat.PreviousPosition) - (team.TeamStat.Position)
                })
                .Where(x => x.Jump > 0) // فلتر اللي طلعوا لفوق بس
                .OrderByDescending(x => x.Jump)
                .FirstOrDefault();

            // 2. عند الـ Return بنأمن الـ Mapping
            if (climber == null || climber.Team == null)
            {
                // رجع كائن فاضي أو قيم افتراضية بدل ما تضرب Error
                new HighstTeamDTO { TeamName = "No Climbers Yet", LogoUrl = "N/A", Value =0};
            }

            return MapToHighlight(climber.Team, climber.Jump);

        }
        //bigest drop
        private HighstTeamDTO GetTopSlider(List<Team> teams)
        {
            var climber = teams
                .Where(t => t.TeamStat != null && t.TeamStat.PreviousPosition > 0) // تأكد إن فيه إحصائيات ومركز سابق
                .Select(team => new
                {
                    Team = team,
                    // بنستخدم الـ Position والـ PreviousPosition بأمان
                    drop = (team.TeamStat.Position) - (team.TeamStat.PreviousPosition)
                })
                .Where(x => x.drop > 0) // فلتر اللي طلعوا لفوق بس
                .OrderByDescending(x => x.drop)
                .FirstOrDefault();

            // 2. عند الـ Return بنأمن الـ Mapping
            if (climber == null || climber.Team == null)
            {
                // رجع كائن فاضي أو قيم افتراضية بدل ما تضرب Error
                new HighstTeamDTO { TeamName = "No drop Yet", LogoUrl = "N/A", Value = 0 };
            }

            return MapToHighlight(climber.Team, climber.drop);

        }
        private HighstTeamDTO MapToHighlight(Team team, int value) =>
        new HighstTeamDTO { TeamName = team.Tname, LogoUrl = team.Logo, Value = value };
        #endregion
    }
}

