using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LinkedOut.Infrastructure.Persistence.Migrations.PostgreSQL
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_searches",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_searches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_search_id = table.Column<int>(type: "integer", nullable: false),
                    location_city_name = table.Column<string>(type: "text", nullable: true),
                    location_province = table.Column<string>(type: "text", nullable: true),
                    organization_name = table.Column<string>(type: "text", nullable: true),
                    job_title = table.Column<string>(type: "text", nullable: true),
                    resume = table.Column<string>(type: "text", nullable: true),
                    cover_letter = table.Column<string>(type: "text", nullable: true),
                    job_description = table.Column<string>(type: "text", nullable: true),
                    job_description_format = table.Column<int>(type: "integer", nullable: false),
                    source = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_applications_job_searches_parent_search_id",
                        column: x => x.parent_search_id,
                        principalTable: "job_searches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    author = table.Column<string>(type: "text", nullable: true),
                    contents = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note", x => x.id);
                    table.ForeignKey(
                        name: "fk_note_job_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "job_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "offer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    extended = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: false),
                    responded = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    details = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "status_transition",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    transition_to = table.Column<int>(type: "integer", nullable: false),
                    transition_from = table.Column<int>(type: "integer", nullable: false),
                    resolution = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_status_transition", x => x.id);
                    table.ForeignKey(
                        name: "fk_status_transition_job_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "job_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_job_applications_parent_search_id",
                table: "job_applications",
                column: "parent_search_id");

            migrationBuilder.CreateIndex(
                name: "ix_note_application_id",
                table: "note",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "ix_offer_application_id",
                table: "offer",
                column: "application_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_status_transition_application_id",
                table: "status_transition",
                column: "application_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "offer");

            migrationBuilder.DropTable(
                name: "status_transition");

            migrationBuilder.DropTable(
                name: "job_applications");

            migrationBuilder.DropTable(
                name: "job_searches");
        }
    }
}
