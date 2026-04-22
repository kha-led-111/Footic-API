using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class Stadium
{
    public int StadiumId { get; set; }

    public string Sname { get; set; } = null!;

    public string Slocation { get; set; } = null!;

    public int Capacity { get; set; }

    public string? LogoUrl { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
