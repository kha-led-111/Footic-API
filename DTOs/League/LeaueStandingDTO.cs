namespace footic.DTOs.League
{
    public class LeaueStandingDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string LogoUrl { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst; // خاصية محسوبة
        public int Points { get; set; }
        public List<char> LastFiveMatches { get; set; } = new List<char>();




    }
}
