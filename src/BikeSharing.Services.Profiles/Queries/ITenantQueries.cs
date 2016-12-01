using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Queries
{
    public interface ITenantQueries
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        Tenant GetById(int id);
        Tenant GetByUserId(int userid);
    }
}
