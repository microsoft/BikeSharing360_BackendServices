using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeSharing.Services.Profiles.Migrations
{
    public partial class ProfileVoicePhrase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoiceSecretPhrase",
                table: "Profiles",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoiceSecretPhrase",
                table: "Profiles");
        }
    }
}
