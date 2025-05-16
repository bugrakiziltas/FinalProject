using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class messagemodellupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Emotion",
                table: "MessageModels",
                newName: "VoiceEmotion");

            migrationBuilder.AddColumn<string>(
                name: "TextEmotion",
                table: "MessageModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextEmotion",
                table: "MessageModels");

            migrationBuilder.RenameColumn(
                name: "VoiceEmotion",
                table: "MessageModels",
                newName: "Emotion");
        }
    }
}
