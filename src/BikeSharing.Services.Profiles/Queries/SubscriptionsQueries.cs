using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Queries
{
    public class SubscriptionsQueries : ISubscriptionsQueries
    {
        private readonly ProfilesDbContext _db;

        public SubscriptionsQueries(ProfilesDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync(int from, int take)
        {
            var data = await _db.Subscriptions.
                Include(s => s.User).
                Include(s => s.User.Profile).
                Include(s => s.User.Profile.Payment).
                Skip(from).
                Take(take).
                ToListAsync();

            return data;

        }
        public async Task<int> CountAsync()
        {
            return await _db.Subscriptions.CountAsync();
        }
    }
}
