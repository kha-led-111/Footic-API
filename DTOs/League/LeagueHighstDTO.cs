namespace footic.DTOs.League
{
    public class LeagueHighstDTO
    {
        public HighstTeamDTO TopScoring { get; set; }      // الأكثر تسجيلاً
        public HighstTeamDTO BestDefense { get; set; }     // الأفضل دفاعاً
        public HighstTeamDTO WorstDefense { get; set; }    // الأكثر استقبالاً
        public HighstTeamDTO BestGD { get; set; }          // أعلى فارق أهداف
        public HighstTeamDTO TopHomeWinner { get; set; }   // ملك الأرض
        public HighstTeamDTO TopAwayWinner { get; set; }   // ملك خارج الأرض
        public HighstTeamDTO TopClimber { get; set; }         // الأكثر صعوداً
        public HighstTeamDTO TopSlider { get; set; }           //الاكثر هبوطا


    }
    
}
