using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonParserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_parser_tasks",
                table: "parser_tasks");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "parser_tasks",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "parser_tasks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProductUrl",
                table: "parser_tasks",
                newName: "product_url");

            migrationBuilder.RenameColumn(
                name: "NextRun",
                table: "parser_tasks",
                newName: "next_run");

            migrationBuilder.RenameColumn(
                name: "LastRun",
                table: "parser_tasks",
                newName: "last_run");

            migrationBuilder.RenameColumn(
                name: "ExternalProductId",
                table: "parser_tasks",
                newName: "external_product_id");

            migrationBuilder.RenameColumn(
                name: "CheckInterval",
                table: "parser_tasks",
                newName: "check_interval");

            migrationBuilder.AddPrimaryKey(
                name: "pk_parser_tasks",
                table: "parser_tasks",
                column: "id");

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occured_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_parser_tasks",
                table: "parser_tasks");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "parser_tasks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "parser_tasks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "product_url",
                table: "parser_tasks",
                newName: "ProductUrl");

            migrationBuilder.RenameColumn(
                name: "next_run",
                table: "parser_tasks",
                newName: "NextRun");

            migrationBuilder.RenameColumn(
                name: "last_run",
                table: "parser_tasks",
                newName: "LastRun");

            migrationBuilder.RenameColumn(
                name: "external_product_id",
                table: "parser_tasks",
                newName: "ExternalProductId");

            migrationBuilder.RenameColumn(
                name: "check_interval",
                table: "parser_tasks",
                newName: "CheckInterval");

            migrationBuilder.AddPrimaryKey(
                name: "PK_parser_tasks",
                table: "parser_tasks",
                column: "Id");
        }
    }
}
