namespace footic.DTOs.player
{
    public class PlayerStatsDTO:PlayerDTO
    {
        public DateOnly Joined { get; set; }

        public DateOnly EndContract { get; set; }

        public int Fit { get; set; }
        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int RedCards { get; set; }

        public int YellowCards { get; set; }



    }
}
