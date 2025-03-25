using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class vmProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "VoiceMessageModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VoiceMessageModels_ProductId",
                table: "VoiceMessageModels",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceMessageModels_Products_ProductId",
                table: "VoiceMessageModels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceMessageModels_Products_ProductId",
                table: "VoiceMessageModels");

            migrationBuilder.DropIndex(
                name: "IX_VoiceMessageModels_ProductId",
                table: "VoiceMessageModels");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "VoiceMessageModels");
        }
    }
}
