namespace footic.DTOs.teams
{
    public class TeamHeaderDTO
    {
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string CoachName { get; set; }
        public string StadiumName { get; set; }
        public string LeagueName { get; set; }

        // الجزء الخاص بـ "Next Match"
        public NextMatchDTO NextMatch { get; set; }

        // الـ Form Guide (الخمس دوائر الملونة)
        public List<char> FormGuide { get; set; } // ['W', 'W', 'W', 'W', 'L']
    }
}
