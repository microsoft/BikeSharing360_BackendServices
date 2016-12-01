using BikeSharing.Models.Profiles;
using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBikes.Services.Profiles.Commands
{
    public class AddSubscriptionCommand : CommandBase
    {
        public int UserId { get; }
        public Subscription Subscription { get; }
        private readonly SubscriptionModel _subsdata;


        public AddSubscriptionCommand(int userid, SubscriptionModel subsdata)
        {
            UserId = userid;
            _subsdata = subsdata;
            Subscription = CreateSubscriptionFromModel(userid, subsdata);
                
        }

        protected override IEnumerable<string> OnValidation()
        {
            if (UserId == 0)
            {
                yield return "Invalid userid";
            }
            if (_subsdata == null)
            {
                yield return "No payload found";
            }
            if (_subsdata != null)
            {
                if (_subsdata.Type == SubscriptionType.None)
                {
                    yield return "No subscription type found";
                }
                else if (_subsdata.Type == SubscriptionType.Member &&
                    _subsdata.Expiration == SubscriptionExpiration.None)
                {
                    yield return "No expiration date found";
                }

            }
        }


        private Subscription CreateSubscriptionFromModel(int userid, SubscriptionModel subsdata)
        {
            var subscription = new Subscription()
            {
                UserId = userid
            };
            if (subsdata.Type == SubscriptionType.Member)
            {
                DateTime today = DateTime.UtcNow.Date;
                switch (subsdata.Expiration)
                {
                    case SubscriptionExpiration.Monthly:
                        subscription.ExpiresOn = today.AddMonths(1);
                        break;
                    case SubscriptionExpiration.Quarterly:
                        subscription.ExpiresOn = today.AddMonths(3);
                        break;
                    case SubscriptionExpiration.Yearly:
                        subscription.ExpiresOn = today.AddYears(1);
                        break;
                }
            }

            subscription.Type = subsdata.Type;
            subscription.Status = SubscriptionStatus.Valid;
            return subscription;
        }
    }
}
