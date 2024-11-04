using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class ActivityProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectActivities_ProjectId1",
                table: "ProjectActivities",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId1",
                table: "ProjectActivities",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId1",
                table: "ProjectActivities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectActivities_ProjectId1",
                table: "ProjectActivities");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "ProjectActivities");
        }
    }
}
