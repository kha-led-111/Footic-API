using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class PlayerStat
{
    public int PlayerStatsId { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public int Goals { get; set; }

    public int Assists { get; set; }

    public int RedCards { get; set; }

    public int YellowCards { get; set; }

    public virtual Player PlayerStats { get; set; } = null!;
}
