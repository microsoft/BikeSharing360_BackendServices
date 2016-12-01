using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Services.Profiles.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly ProfilesDbContext _db;

        public UserQueries(ProfilesDbContext ctx)
        {
            _db = ctx;
        }

        public User GetUserById(int userid)
        {
            var user = _db.Users.SingleOrDefault(u => u.Id == userid);
            return user;
        }

        public IEnumerable<User> GetUsersById(IEnumerable<int> users)
        {
            return _db.Users.Include(u => u.Profile)
                            .Where(u => users.Contains(u.Id));
        }
    }
}
