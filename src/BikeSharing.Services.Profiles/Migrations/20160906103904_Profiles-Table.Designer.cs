using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BikeSharing.Services.Profiles.Data;

namespace BikeSharing.Services.Profiles.Migrations
{
    [DbContext(typeof(ProfilesDbContext))]
    [Migration("20160906103904_Profiles-Table")]
    partial class ProfilesTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikeSharing.Models.Profiles.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId");

                    b.ToTable("Profiles");
                });
        }
    }
}
