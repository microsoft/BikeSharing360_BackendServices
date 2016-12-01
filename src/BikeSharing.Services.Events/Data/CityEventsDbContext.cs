using BikeSharing.Models.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Events.Data
{
    public class CityEventsDbContext : DbContext
    {
        public DbSet<CityEvent> Events { get; set; }
        public DbSet<EventVenue> Venues { get; set; }
        public DbSet<Classification> Classifications { get; set; }


        public CityEventsDbContext(DbContextOptions<CityEventsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CityEvent>().
                HasOne<Classification>(ce => ce.Segment)
                .WithMany((string)null)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            modelBuilder.Entity<CityEvent>().
                HasOne<Classification>(ce => ce.Genre)
                .WithMany((string)null)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);
            modelBuilder.Entity<CityEvent>().
                HasOne<Classification>(ce => ce.SubGenre)
                .WithMany((string)null)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            modelBuilder.Entity<EventVenue>().
                HasAlternateKey(v => v.ExternalId);

            modelBuilder.Entity<Classification>().
                HasAlternateKey(c => c.ExternalId);

            modelBuilder.Entity<CityEvent>().
                HasAlternateKey(e => e.ExternalId);

            modelBuilder.Entity<EventVenue>().
                Property(v => v.Latitude).HasColumnType("numeric(18,10)");
            modelBuilder.Entity<EventVenue>().
                Property(v => v.Longitude).HasColumnType("numeric(18,10)");
        }
    }
}
