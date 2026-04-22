using footic.EData;

namespace footic.DTOs.Match
{
    public class MatchDTO
    {
        public int Id {  get; set; }
        public int HomeTeamID {  get; set; }
        public int AwayTeamID {  get; set; }
        public int ?HomeTeamScore {  get; set; }
        public int ?AwayTeamScore { get;set; }
        public MatchState MatchState { get; set; }
        public DateTime Matchdate { get; set; }
        public TimeOnly Matchtime {  get; set; }
        public int stadiumID {  get; set; } 
        public int week {  get; set; }



    }
}
