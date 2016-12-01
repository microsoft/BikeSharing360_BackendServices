using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Queries
{
    public interface IUserQueries
    {
        User GetUserById(int userid);
        IEnumerable<User> GetUsersById(IEnumerable<int> userid);
    }
}
