using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LinkedOut.Infrastructure.Persistence.Migrations.PostgreSQL
{
    public partial class AddIsRemote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offer");

            migrationBuilder.AddColumn<bool>(
                name: "is_remote",
                table: "job_applications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_remote",
                table: "job_applications");

            migrationBuilder.CreateTable(
                name: "offer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    details = table.Column<string>(type: "text", nullable: true),
                    expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    extended = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true),
                    responded = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offer", x => x.id);
                    table.ForeignKey(
                        name: "fk_offer_job_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "job_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_offer_application_id",
                table: "offer",
                column: "application_id",
                unique: true);
        }
    }
}
