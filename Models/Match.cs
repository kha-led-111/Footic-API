using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using footic.EData;

namespace footic.Models;

public partial class Match
{
    [Key]
    public int MatchId { get; set; }

    [Required]
    public DateTime MatchDate { get; set; }
    public TimeOnly MatchTime { get; set; }

    // --- علاقات الفرق ---

    [Required]
    public int HomeTeamId { get; set; }

    [ForeignKey("HomeTeamId")]
    public virtual Team HomeTeam { get; set; } = null!;

    [Required]
    public int AwayTeamId { get; set; }

    [ForeignKey("AwayTeamId")]
    public virtual Team AwayTeam { get; set; } = null!;

    // --- النتائج والحالة ---

    public byte HomeTeamScore { get; set; } = 0;

    public byte AwayTeamScore { get; set; } = 0;

    [Required]
    public MatchState Status { get; set; } = MatchState.Upcoming;

    // --- الربط مع الدوري والستاد ---

    [Required]
    public int LeagueId { get; set; }

    [ForeignKey("LeagueId")]
    public virtual League League { get; set; } = null!;

    public int? StadiumId { get; set; }

    [ForeignKey("StadiumId")]
    public virtual Stadium? Stadium { get; set; }

    public int Week { get; set; }
}
