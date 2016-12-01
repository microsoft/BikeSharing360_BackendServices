using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BikeSharing.Services.Profiles.Data;

namespace BikeSharing.Services.Profiles.Migrations
{
    [DbContext(typeof(ProfilesDbContext))]
    [Migration("20160922073403_NewSchemaAndPayment")]
    partial class NewSchemaAndPayment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikeSharing.Models.Profiles.PaymentData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreditCard")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("CreditCardType");

                    b.Property<DateTime>("ExpirationDate");

                    b.HasKey("Id");

                    b.ToTable("PaymentData");
                });

            modelBuilder.Entity("BikeSharing.Models.Profiles.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ExpiresOn");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Subscription");
                });

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

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int?>("PaymentId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("BikeSharing.Models.Profiles.Subscription", b =>
                {
                    b.HasOne("BikeSharing.Models.Profiles.User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BikeSharing.Models.Profiles.UserProfile", b =>
                {
                    b.HasOne("BikeSharing.Models.Profiles.PaymentData", "Payment")
                        .WithOne()
                        .HasForeignKey("BikeSharing.Models.Profiles.UserProfile", "PaymentId");

                    b.HasOne("BikeSharing.Models.Profiles.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("BikeSharing.Models.Profiles.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
