using BikeSharing.Services.Feedback.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Data
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options)
        {

        }

        public DbSet<ReportedIssue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetStringMaxLengthConvention(1024);
        }


    }
}
