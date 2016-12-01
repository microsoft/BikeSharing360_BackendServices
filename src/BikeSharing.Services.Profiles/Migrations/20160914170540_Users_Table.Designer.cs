using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BikeSharing.Services.Profiles.Data;

namespace BikeSharing.Services.Profiles.Migrations
{
    [DbContext(typeof(ProfilesDbContext))]
    [Migration("20160914170540_Users_Table")]
    partial class Users_Table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikeSharing.Models.Profiles.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

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

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("BikeSharing.Models.Profiles.UserProfile", b =>
                {
                    b.HasOne("BikeSharing.Models.Profiles.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("BikeSharing.Models.Profiles.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
