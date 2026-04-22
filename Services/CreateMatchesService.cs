//using footic.DTOs.Match;
//using footic.EData;
//using footic.Models;
//using Microsoft.EntityFrameworkCore;

//namespace footic.Services
//{
//    public class CreateMatchesService
//    {
//        private readonly PlSimulationDbContext _context;
//        public CreateMatchesService(PlSimulationDbContext context)
//        {
//            _context = context;
//        }

//        //function to get data from DB
//        //teams
//        private List<Team> allteams = new List<Team>();


//        private static Dictionary<int, int> STADIUMS = new Dictionary<int, int>();
//        public async Task assigndata()
//        {
//            allteams = await _context.Teams.Include(t => t.Stadium).ToListAsync();
//            foreach (var team in allteams)
//            {
//                STADIUMS.Add(team.TeamId, team.StadiumId ?? 0);
//            }


//        }


//        //{
//        //    {"Al Ahly","Cairo International Stadium"},
//        //    {"Zamalek","Cairo International Stadium"},
//        //    {"Pyramids","30 June Stadium"},
//        //    {"Al Masry","Suez Stadium"},
//        //    {"Ceramica Cleopatra","Osman Ahmed Osman Stadium"},
//        //    {"Al Bank Al Ahly","Military College Stadium"},
//        //    {"Al Ittihad","Alexandria Stadium"},
//        //    {"ZED","Canal Suez Stadium"},
//        //    {"Modern Sport","Salam Stadium"},
//        //    {"Wadi Degla","Petro Sport Stadium"},
//        //    {"Petrojet","Military Academy Stadium"},
//        //    {"ENPPI","Petro Sport Stadium"},
//        //    {"Ismaily","Ismailia Stadium"},
//        //    {"Al Mokawloon","Osman Ahmed Osman Stadium"},
//        //    {"Smouha","Borg El Arab Stadium"},
//        //    {"Tala'ea El Gaish","Gehaz El Reyada Stadium"},
//        //    {"Ghazl El Mahalla","Ghazl El Mahalla Stadium"},
//        //    {"Farco","Haras El Hodoud Stadium"},
//        //};

//        private static DateTime SEASON_START = new DateTime(2025, 8, 8);
//        private static DateTime SEASON_END = new DateTime(2026, 6, 7);

//        private static HashSet<int> PREFERRED_DAYS = new HashSet<int> { 3, 4, 5 };

//        private static List<string> DAY_TIMES = new List<string> { "16:00", "20:00", "22:00" };
//        private static List<string> EXTRA_TIMES = new List<string> { "14:00", "18:00" };


//        private Dictionary<int, DateTime> team_last_match = new();
//        private Dictionary<int, int> team_home_streak = new();
//        private Dictionary<(int, DateTime), HashSet<TimeOnly>> stadium_day_times = new();

//        private List<DateTime> GetWindow(int roundIdx, int size = 5)
//        {
//            var baseDate = SEASON_START.AddDays(roundIdx * 7);

//            for (int offset = 0; offset < 28; offset++)
//            {
//                var start = baseDate.AddDays(offset);
//                if (start < SEASON_START || start > SEASON_END.AddDays(-2))
//                    continue;

//                var window = new List<DateTime>();
//                for (int d = 0; d < size; d++)
//                {
//                    var day = start.AddDays(d);
//                    if (day >= SEASON_START && day <= SEASON_END)
//                        window.Add(day);
//                }

//                if (window.Count >= 3)
//                    return window;
//            }

//            return Enumerable.Range(0, size)
//                .Select(i => SEASON_START.AddDays(roundIdx * 7 + i))
//                .ToList();
//        }

//        private bool Check72h(int teamid, DateTime newDate, List<(int, int, DateTime)> temp)
//        {
//            if (team_last_match.ContainsKey(teamid))
//            {
//                var last = team_last_match[teamid];
//                if (Math.Abs((newDate - last).TotalDays) < 3)
//                    return false;
//            }

//            foreach (var m in temp)
//            {
//                if (m.Item1 == teamid || m.Item2 == teamid)
//                {
//                    if (Math.Abs((newDate - m.Item3).TotalDays) < 3)
//                        return false;
//                }
//            }

//            return true;
//        }


//        private (List<List<(int, int)>>, List<List<(int, int)>>) MakeRounds()
//        {
//            int n = allteams.Count;
//            int half = n / 2;

//            var rotation = Enumerable.Range(0, n).ToList();
//            var first_leg = new List<List<(int, int)>>();

//            for (int r = 0; r < n - 1; r++)
//            {
//                var round = new List<(int, int)>();

//                for (int i = 0; i < half; i++)
//                    round.Add((rotation[i], rotation[n - 1 - i]));

//                first_leg.Add(round);

//                rotation = new List<int> { rotation[0], rotation[^1] }
//                    .Concat(rotation.Skip(1).Take(n - 2)).ToList();
//            }

//            var second_leg = first_leg
//                .Select(r => r.Select(p => (p.Item2, p.Item1)).ToList())
//                .ToList();

//            return (first_leg, second_leg);
//        }

//        private List<List<(int, int)>> Interleave(List<List<(int, int)>> first_leg, List<List<(int, int)>> second_leg)
//        {
//            var result = new List<List<(int, int)>>();

//            for (int i = 0; i < first_leg.Count; i++)
//            {
//                result.Add(first_leg[i]);
//                result.Add(second_leg[i]);
//            }

//            return result;
//        }

//        private (int h, int a, DateTime d, TimeOnly t, int s)? AssignMatch(
//            int homeID,
//            int awayID,
//            List<DateTime> days,
//            Dictionary<TimeOnly, List<string>> used,
//            List<(int, int, DateTime)> temp)
//        {
//            var sortedDays = days
//                .OrderBy(d => used[d].Count)
//                .ThenBy(d => PREFERRED_DAYS.Contains((int)d.DayOfWeek) ? 0 : 1)
//                .ThenBy(d => d)
//                .ToList();

//            foreach (var day in sortedDays)
//            {
//                if (used[day].Count >= 3)
//                    continue;

//                if (!Check72h(homeID, day, temp) || !Check72h(awayID, day, temp))
//                    continue;

//                var freeTimes = DAY_TIMES.Where(t => !used[day].Contains(t)).ToList();

//                if (!freeTimes.Any())
//                    freeTimes = EXTRA_TIMES.Where(t => !used[day].Contains(t)).ToList();

//                if (!freeTimes.Any())
//                    continue;

//                var stadium = STADIUMS[homeID];

//                foreach (var time in freeTimes)
//                {
//                    //if (stadium_day_times.TryGetValue((stadium, day), out var booked) && booked.Contains(time))
//                    //    continue;

//                    //return (homeID, awayID, day, TimeOnly.Parse(time), stadium);
//                    var key = (stadium, day);

//                    if (stadium_day_times.ContainsKey(key) &&
//                        stadium_day_times[key].Contains(time))
//                        continue;

//                    return (homeID, awayID, day, time, stadium);
//                }
//            }

//            return null;
//        }

//        private List<Match> Schedule()
//        {
//            var (first_leg, second_leg) = MakeRounds();
//            var rounds = Interleave(first_leg, second_leg);

//            var solution = new List<Match>();

//            foreach (var team in allteams)
//                team_home_streak[team.TeamId] = 0;

//            for (int r = 0; r < rounds.Count; r++)
//            {
//                var pairs = rounds[r]
//                    .Select(p => (allteams[p.Item1].TeamId, allteams[p.Item2].TeamId))
//                    .ToList();

//                var window = GetWindow(r);
//                var used = window.ToDictionary(d => d, d => new List<TimeOnly>());
//                var temp = new List<(int, int, DateTime)>();

//                foreach (var (homeID, awayID) in pairs)
//                {
//                    var res = AssignMatch(homeID, awayID, window, used, temp);

//                    if (res != null)
//                    {
//                        var (h, a, d, t, s) = res.Value;

//                        temp.Add((h, a, d));
//                        used[d].Add(t);

//                        solution.Add(new Match //match
//                        {
//                            Week = r + 1,
//                            MatchDate = d,
//                            MatchTime =t,
//                            HomeTeamId = h,
//                            AwayTeamId = a,
//                            StadiumId = s,
//                            Status=0,


//                        });

//                        team_last_match[h] = d;
//                        team_last_match[a] = d;

//                        if (!stadium_day_times.ContainsKey((s, d)))
//                            stadium_day_times[(s, d)] = new HashSet<ti>();

//                        stadium_day_times[(s, d)].Add(t);

//                        team_home_streak[h]++;
//                        team_home_streak[a] = 0;
//                    }
//                }
//            }

//            return solution;
//        }

//        public List<Match> GenerateSchedule()
//        {
//            return Schedule();
//        }


//    }
//}


using footic.DTOs.Match;
using footic.EData;
using footic.Models;
using Microsoft.EntityFrameworkCore;

namespace footic.Services
{
    public class CreateMatchesService
    {
        private readonly PlSimulationDbContext _context;
        private List<Team> allteams = new List<Team>();
        private static Dictionary<int, int> STADIUMS = new Dictionary<int, int>();

        // إعدادات الموسم
        private static DateTime SEASON_START = new DateTime(2025, 8, 8);
        private static DateTime SEASON_END = new DateTime(2026, 6, 7);
        private static HashSet<int> PREFERRED_DAYS = new HashSet<int> { 3, 4, 5 };

        // الأوقات كـ string لسهولة التعامل مع الـ Parse
        private static List<string> DAY_TIMES = new List<string> { "16:00", "20:00", "22:00" };
        private static List<string> EXTRA_TIMES = new List<string> { "14:00", "18:00" };

        private Dictionary<int, DateTime> team_last_match = new();
        private Dictionary<int, int> team_home_streak = new();
        private Dictionary<(int, DateTime), HashSet<string>> stadium_day_times = new();

        public CreateMatchesService(PlSimulationDbContext context)
        {
            _context = context;
        }

        // جلب البيانات الأساسية من الداتا بيز
        public async Task InitializeData()
        {
            allteams = await _context.Teams.AsNoTracking().ToListAsync();
            STADIUMS.Clear();
            foreach (var team in allteams)
            {
                STADIUMS.Add(team.TeamId, team.StadiumId ?? 0);
            }
        }

        private List<DateTime> GetWindow(int roundIdx, int size = 5)
        {
            var baseDate = SEASON_START.AddDays(roundIdx * 7);
            for (int offset = 0; offset < 28; offset++)
            {
                var start = baseDate.AddDays(offset);
                if (start < SEASON_START || start > SEASON_END.AddDays(-2)) continue;

                var window = new List<DateTime>();
                for (int d = 0; d < size; d++)
                {
                    var day = start.AddDays(d);
                    if (day >= SEASON_START && day <= SEASON_END) window.Add(day);
                }
                if (window.Count >= 3) return window;
            }
            return Enumerable.Range(0, size).Select(i => SEASON_START.AddDays(roundIdx * 7 + i)).ToList();
        }

        private bool Check72h(int teamid, DateTime newDate, List<(int, int, DateTime)> temp)
        {
            if (team_last_match.ContainsKey(teamid))
            {
                if (Math.Abs((newDate - team_last_match[teamid]).TotalDays) < 3) return false;
            }
            foreach (var m in temp)
            {
                if (m.Item1 == teamid || m.Item2 == teamid)
                {
                    if (Math.Abs((newDate - m.Item3).TotalDays) < 3) return false;
                }
            }
            return true;
        }

        private (List<List<(int, int)>>, List<List<(int, int)>>) MakeRounds()
        {
            int n = allteams.Count;
            if (n % 2 != 0) return (new(), new()); // حماية لو العدد فردي

            var indices = Enumerable.Range(0, n).ToList();
            var first_leg = new List<List<(int, int)>>();

            for (int r = 0; r < n - 1; r++)
            {
                var round = new List<(int, int)>();
                for (int i = 0; i < n / 2; i++)
                    round.Add((indices[i], indices[n - 1 - i]));

                first_leg.Add(round);
                indices.Insert(1, indices[^1]);
                indices.RemoveAt(indices.Count - 1);
            }

            var second_leg = first_leg.Select(r => r.Select(p => (p.Item2, p.Item1)).ToList()).ToList();
            return (first_leg, second_leg);
        }

        private List<List<(int, int)>> Interleave(List<List<(int, int)>> first, List<List<(int, int)>> second)
        {
            var result = new List<List<(int, int)>>();
            for (int i = 0; i < first.Count; i++)
            {
                result.Add(first[i]);
                result.Add(second[i]);
            }
            return result;
        }

        // الدالة المسؤولة عن تخصيص وقت ومكان للمباراة
        private (int h, int a, DateTime d, TimeOnly t, int s)? AssignMatch(
            int homeID, int awayID, List<DateTime> days,
            Dictionary<DateTime, List<TimeOnly>> used, List<(int, int, DateTime)> temp)
        {
            var sortedDays = days.OrderBy(d => used[d].Count)
                .ThenBy(d => PREFERRED_DAYS.Contains((int)d.DayOfWeek) ? 0 : 1).ToList();

            foreach (var day in sortedDays)
            {
                if (used[day].Count >= 3) continue;
                if (!Check72h(homeID, day, temp) || !Check72h(awayID, day, temp)) continue;

                // تحويل النصوص لـ TimeOnly للبحث
                var allAvailable = DAY_TIMES.Concat(EXTRA_TIMES).Select(TimeOnly.Parse).ToList();
                var freeTimes = allAvailable.Where(t => !used[day].Contains(t)).ToList();

                var stadium = STADIUMS[homeID];

                foreach (var time in freeTimes)
                {
                    if (stadium_day_times.TryGetValue((stadium, day), out var booked) && booked.Contains(time.ToString()))
                        continue;

                    return (homeID, awayID, day, time, stadium);
                }
            }
            return null;
        }

        public List<displayMatchDto> Schedule()
        {
            var (first_leg, second_leg) = MakeRounds();
            var rounds = Interleave(first_leg, second_leg);
            var solution = new List<displayMatchDto>();

            foreach (var team in allteams) team_home_streak[team.TeamId] = 0;

            for (int r = 0; r < rounds.Count; r++)
            {
                // تأكد من جلب الـ IDs الحقيقية من قائمة allteams
                var pairs = rounds[r]
                    .Select(p => (allteams[p.Item1].TeamId, allteams[p.Item2].TeamId))
                    .ToList();

                var window = GetWindow(r);
                var used = window.ToDictionary(d => d, d => new List<TimeOnly>());
                var temp = new List<(int, int, DateTime)>();

                foreach (var (homeID, awayID) in pairs)
                {
                    var res = AssignMatch(homeID, awayID, window, used, temp);
                    if (res != null)
                    {
                        var (h, a, d, t, s) = res.Value;
                        temp.Add((h, a, d));
                        used[d].Add(t);

                        solution.Add(new displayMatchDto
                        {
                            week = r + 1,
                            Matchdate = d,
                            Matchtime = t,
                            HomeTeamID = h,
                            Hometeamname = GetTeamName(h), // هنا الاسم للعرض
                            AwayTeamID = a,
                            Awayteamname = GetTeamName(a), // هنا الاسم للعرض
                            stadiumID = s,
                            MatchState = MatchState.Upcoming
                        });

                        team_last_match[h] = d;
                        team_last_match[a] = d;

                        if (!stadium_day_times.ContainsKey((s, d)))
                            stadium_day_times[(s, d)] = new HashSet<string>();

                        stadium_day_times[(s, d)].Add(t.ToString());
                    }
                }
            }
            return solution;
                 
        }
        public async Task<int> SaveGeneratedMatches(List<displayMatchDto> matchesDto)
        {
            // تحويل الـ DTOs إلى Entities (Match)
            var matchesToSave = matchesDto.Select(dto => new Match
            {
                // الـ MatchId هيتولد تلقائي في الداتابيز
                MatchDate = dto.Matchdate,
                MatchTime = dto.Matchtime,
                HomeTeamId = dto.HomeTeamID,
                AwayTeamId = dto.AwayTeamID,
                StadiumId = dto.stadiumID,
                Week = dto.week,

                // قيم افتراضية مطلوبة في الموديل بتاعك
                Status = MatchState.Upcoming, // أو القيمة الرقمية لـ 0
                LeagueId = 1, // تأكد من وجود دوري برقم 1 في قاعدة بياناتك
                HomeTeamScore = 0,
                AwayTeamScore = 0
            }).ToList();

            await _context.Matches.AddRangeAsync(matchesToSave);
            return await _context.SaveChangesAsync(); // بيرجع عدد الماتشات اللي اتسيفت فعلياً
        }
        private string GetTeamName(int teamId)
        {
            return allteams.FirstOrDefault(t => t.TeamId == teamId)?.Tname ?? "Unknown";
        }
    }
}