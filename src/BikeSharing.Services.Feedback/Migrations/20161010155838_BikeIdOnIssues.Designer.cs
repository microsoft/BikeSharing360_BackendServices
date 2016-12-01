using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BikeSharing.Services.Feedback.Api.Data;

namespace BikeSharing.Services.Feedback.Api.Migrations
{
    [DbContext(typeof(FeedbackDbContext))]
    [Migration("20161010155838_BikeIdOnIssues")]
    partial class BikeIdOnIssues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikeSharing.Services.Feedback.Api.Models.ReportedIssue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BikeId");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<int>("IssueType");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<bool>("Solved");

                    b.Property<int?>("StopId");

                    b.Property<int>("UserId");

                    b.Property<DateTime>("UtcTime");

                    b.HasKey("Id");

                    b.ToTable("Issues");
                });
        }
    }
}
