using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Queries
{
    public interface ISubscriptionsQueries
    {
        Task<IEnumerable<Subscription>> GetAllAsync(int from, int take);
        Task<int> CountAsync();
    }
}
