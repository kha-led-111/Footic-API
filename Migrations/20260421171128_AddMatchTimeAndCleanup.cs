using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace footic.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchTimeAndCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Match_ DROP COLUMN IF EXISTS TeamId;");
            migrationBuilder.Sql("ALTER TABLE Match_ DROP COLUMN IF EXISTS TeamId1;");

            // 2. معالجة علاقة TeamStats (حذف القديم وإضافة الجديد بـ Cascade)
            migrationBuilder.DropForeignKey(
                name: "FK_TeamStats_Team",
                table: "TeamStats");

            // 3. إضافة عمود الوقت الجديد
            migrationBuilder.AddColumn<TimeOnly>(
                name: "MatchTime",
                table: "Match_",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            // 4. إعادة بناء الـ ForeignKey لـ TeamStats
            migrationBuilder.AddForeignKey(
                name: "FK_TeamStats_Team",
                table: "TeamStats",
                column: "TeamStatsID",
                principalTable: "Team",
                principalColumn: "TeamID",
                onDelete: ReferentialAction.Cascade);
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamStats_Team",
                table: "TeamStats");

            migrationBuilder.DropColumn(
                name: "MatchTime",
                table: "Match_");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Match_",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "Match_",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match__TeamId",
                table: "Match_",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Match__TeamId1",
                table: "Match_",
                column: "TeamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Match__Team_TeamId",
                table: "Match_",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Match__Team_TeamId1",
                table: "Match_",
                column: "TeamId1",
                principalTable: "Team",
                principalColumn: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamStats_Team",
                table: "TeamStats",
                column: "TeamStatsID",
                principalTable: "Team",
                principalColumn: "TeamID");
        }
    }
}
