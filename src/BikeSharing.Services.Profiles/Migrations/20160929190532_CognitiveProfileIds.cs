using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeSharing.Services.Profiles.Migrations
{
    public partial class CognitiveProfileIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FaceProfileId",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VoiceProfileId",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceProfileId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "VoiceProfileId",
                table: "Profiles");
        }
    }
}
