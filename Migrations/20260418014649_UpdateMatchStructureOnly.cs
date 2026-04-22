using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace footic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchStructureOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "League",
            //    columns: table => new
            //    {
            //        LeagueID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        StartDate = table.Column<DateOnly>(type: "date", nullable: true),
            //        EndDate = table.Column<DateOnly>(type: "date", nullable: true),
            //        Winner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_League", x => x.LeagueID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Stadium",
            //    columns: table => new
            //    {
            //        StadiumID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        SLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //        Capacity = table.Column<int>(type: "int", nullable: false),
            //        Logo_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Stadium", x => x.StadiumID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Team",
            //    columns: table => new
            //    {
            //        TeamID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //        Coach = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        StadiumID = table.Column<int>(type: "int", nullable: true),
            //        LeagueID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Team", x => x.TeamID);
            //        table.ForeignKey(
            //            name: "FK_Team_League",
            //            column: x => x.LeagueID,
            //            principalTable: "League",
            //            principalColumn: "LeagueID");
            //        table.ForeignKey(
            //            name: "FK_Team_Stadium",
            //            column: x => x.StadiumID,
            //            principalTable: "Stadium",
            //            principalColumn: "StadiumID");
            //    });

            migrationBuilder.CreateTable(
                name: "Match_",
                columns: table => new
                {
                    MatchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamScore = table.Column<byte>(type: "tinyint", nullable: false),
                    AwayTeamScore = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LeagueID = table.Column<int>(type: "int", nullable: false),
                    StadiumID = table.Column<int>(type: "int", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: false),
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.MatchID);
                    table.ForeignKey(
                        name: "FK_Match_AwayTeam",
                        column: x => x.AwayTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_HomeTeam",
                        column: x => x.HomeTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_League",
                        column: x => x.LeagueID,
                        principalTable: "League",
                        principalColumn: "LeagueID");
                    table.ForeignKey(
                        name: "FK_Match_Stadium",
                        column: x => x.StadiumID,
                        principalTable: "Stadium",
                        principalColumn: "StadiumID",
                        onDelete: ReferentialAction.SetNull);
                   
                });

            //migrationBuilder.CreateTable(
            //    name: "Player",
            //    columns: table => new
            //    {
            //        PlayerID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        PNumber = table.Column<int>(type: "int", nullable: false),
            //        Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Age = table.Column<int>(type: "int", nullable: false),
            //        Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        StrongFoot = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Joined = table.Column<DateOnly>(type: "date", nullable: false),
            //        EndContract = table.Column<DateOnly>(type: "date", nullable: false),
            //        Fit = table.Column<int>(type: "int", nullable: false, defaultValue: 100),
            //        Poweer = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
            //        MarketValue = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
            //        PImage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, defaultValueSql: "((0))"),
            //        TeamID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Player", x => x.PlayerID);
            //        table.ForeignKey(
            //            name: "FK_Player_Team",
            //            column: x => x.TeamID,
            //            principalTable: "Team",
            //            principalColumn: "TeamID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamStats",
            //    columns: table => new
            //    {
            //        TeamStatsID = table.Column<int>(type: "int", nullable: false),
            //        Points = table.Column<int>(type: "int", nullable: false),
            //        GoalsFor = table.Column<int>(type: "int", nullable: false),
            //        GoalsAgainst = table.Column<int>(type: "int", nullable: false),
            //        WinsNumber = table.Column<int>(type: "int", nullable: false),
            //        LoseNumber = table.Column<int>(type: "int", nullable: false),
            //        DrawNumber = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamStats", x => x.TeamStatsID);
            //        table.ForeignKey(
            //            name: "FK_TeamStats_Team",
            //            column: x => x.TeamStatsID,
            //            principalTable: "Team",
            //            principalColumn: "TeamID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        UserID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        UPassword = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        UserType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "fan"),
            //        Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //        TeamID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.UserID);
            //        table.ForeignKey(
            //            name: "FK_Users_Team",
            //            column: x => x.TeamID,
            //            principalTable: "Team",
            //            principalColumn: "TeamID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PlayerStats",
            //    columns: table => new
            //    {
            //        PlayerStatsID = table.Column<int>(type: "int", nullable: false),
            //        Height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
            //        Weight_ = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
            //        Goals = table.Column<int>(type: "int", nullable: false),
            //        Assists = table.Column<int>(type: "int", nullable: false),
            //        RedCards = table.Column<int>(type: "int", nullable: false),
            //        YellowCards = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PlayerStats", x => x.PlayerStatsID);
            //        table.ForeignKey(
            //            name: "FK_PlayerStats_Player",
            //            column: x => x.PlayerStatsID,
            //            principalTable: "Player",
            //            principalColumn: "PlayerID");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "UQ_League_Name",
            //    table: "League",
            //    column: "LName",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__AwayTeamId",
            //    table: "Match_",
            //    column: "AwayTeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__HomeTeamId",
            //    table: "Match_",
            //    column: "HomeTeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__LeagueID",
            //    table: "Match_",
            //    column: "LeagueID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__StadiumID",
            //    table: "Match_",
            //    column: "StadiumID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__TeamId",
            //    table: "Match_",
            //    column: "TeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Match__TeamId1",
            //    table: "Match_",
            //    column: "TeamId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Player_TeamID",
            //    table: "Player",
            //    column: "TeamID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ_Stadium_Name",
            //    table: "Stadium",
            //    column: "SName",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Team_LeagueID",
            //    table: "Team",
            //    column: "LeagueID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Team_StadiumID",
            //    table: "Team",
            //    column: "StadiumID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ_Team_Name",
            //    table: "Team",
            //    column: "TName",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_TeamID",
            //    table: "Users",
            //    column: "TeamID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ_Users_Email",
            //    table: "Users",
            //    column: "Email",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "UQ_Users_UserName",
            //    table: "Users",
            //    column: "UserName",
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match_");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "TeamStats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "League");

            migrationBuilder.DropTable(
                name: "Stadium");
        }
    }
}
