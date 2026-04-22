using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class League
{
    public int LeagueId { get; set; }

    public string Lname { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Winner { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
