using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeSharing.Services.Profiles.Migrations
{
    public partial class ProfileSkype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Profiles",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skype",
                table: "Profiles",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Skype",
                table: "Profiles");
        }
    }
}
