using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Profiles
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public DateTime LastLogin { get; set; }

        public UserProfile Profile { get; set; }

        public string Password { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

    }
}
