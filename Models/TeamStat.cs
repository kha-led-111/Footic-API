using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class TeamStat
{
    public int TeamStatsId { get; set; }

    public int Points { get; set; }

    public int GoalsFor { get; set; }

    public int GoalsAgainst { get; set; }

    public int WinsNumber { get; set; }

    public int LoseNumber { get; set; }

    public int DrawNumber { get; set; }

    public int Position { get; set; } = 0;
    public int PreviousPosition { get; set; } = 0;
    public virtual Team TeamStats { get; set; } = null!;
}
