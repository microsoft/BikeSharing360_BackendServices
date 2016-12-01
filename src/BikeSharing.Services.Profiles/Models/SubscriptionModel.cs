using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBikes.Services.Profiles.Models
{
    public class SubscriptionModel
    {
        public SubscriptionType Type { get; set; }
        public SubscriptionExpiration Expiration { get; set; }
    }
}
