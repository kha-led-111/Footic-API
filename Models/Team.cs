using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string Tname { get; set; } = null!;

    public string? Logo { get; set; }

    public string? Coach { get; set; }

    public int? StadiumId { get; set; }

    public int? LeagueId { get; set; }

    public virtual League? League { get; set; }

    //public virtual ICollection<Match> MatchLoserTeams { get; set; } = new List<Match>();

    //public virtual ICollection<Match> MatchWinnerTeams { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual Stadium? Stadium { get; set; }

    public virtual TeamStat? TeamStat { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Match> HomeMatches { get; set; } = new List<Match>();
    public virtual ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}
