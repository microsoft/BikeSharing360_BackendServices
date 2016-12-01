using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BikeSharing.Services.Profiles.Migrations
{
    public partial class NewSchemaAndPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreditCard = table.Column<string>(maxLength: 255, nullable: true),
                    CreditCardType = table.Column<int>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpiresOn = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Profiles",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Profiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PaymentId",
                table: "Profiles",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UserId",
                table: "Subscription",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_PaymentData_PaymentId",
                table: "Profiles",
                column: "PaymentId",
                principalTable: "PaymentData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_PaymentData_PaymentId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PaymentId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "PaymentData");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
