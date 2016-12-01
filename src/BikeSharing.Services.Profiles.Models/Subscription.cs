using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Profiles
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionType Type { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public SubscriptionStatus Status {get; set;}
        public int UserId { get; set; }

        public User User { get; set; }

        public Subscription()
        {
            Status = SubscriptionStatus.WithError;
        }
    }
}
