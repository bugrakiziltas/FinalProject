using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class confidenceRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "textConfidenceRate",
                table: "MessageModels",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "voiceConfidenceRate",
                table: "MessageModels",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "textConfidenceRate",
                table: "MessageModels");

            migrationBuilder.DropColumn(
                name: "voiceConfidenceRate",
                table: "MessageModels");
        }
    }
}
