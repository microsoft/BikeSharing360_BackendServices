using Microsoft.EntityFrameworkCore;
using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBikes.Models.Profiles;

namespace BikeSharing.Services.Profiles.Data
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetStringMaxLengthConvention(255);

            // One to one between users and profiles where FK is on
            // profiles (UserId)
            modelBuilder.Entity<UserProfile>()
                .HasOne<User>(c => c.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<UserProfile>()
                .Ignore(p => p.PhotoUrl);

            modelBuilder.Entity<UserProfile>()
                .HasOne<PaymentData>(up => up.Payment)
                .WithOne((string)null)
                .HasForeignKey<UserProfile>(up => up.PaymentId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithOne((string)null)
                .HasForeignKey<Subscription>(s => s.UserId);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Subscriptions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Subscription>()
                .ForSqlServerToTable("Subscription");

            modelBuilder.Entity<User>()
                .Property(u => u.TenantId).
                ForSqlServerHasDefaultValue(0);

            modelBuilder.Entity<Tenant>()
                .HasMany<User>(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId);
        }

        public DbSet<UserProfile> Profiles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<PaymentData> PaymentData { get; set; }
    }
}
