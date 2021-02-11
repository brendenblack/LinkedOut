using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkedOut.Infrastructure.Persistence.Migrations.SQLServer
{
    public partial class AddIsRemote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemote",
                table: "JobApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemote",
                table: "JobApplications");

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Extended = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offer_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ApplicationId",
                table: "Offer",
                column: "ApplicationId",
                unique: true);
        }
    }
}
