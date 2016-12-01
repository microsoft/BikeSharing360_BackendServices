using BikeSharing.Models.Profiles;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Profiles.Queries;
using BikeSharing.Servicesubscription.Profilesubscription.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Controllers
{
    [Route("api/[controller]")]
    public class SubscriptionsController : BaseApiController
    {
        private readonly ISubscriptionsQueries _queries;

        public SubscriptionsController(ISubscriptionsQueries queries, ICommandBus bus)
            : base(bus)
        {
            _queries = queries;
        }
        
        [HttpGet()]
        public async Task<IActionResult> GetAll(int from, int take=20)
        {
            var count = await _queries.CountAsync();
            var subscriptions = await _queries.GetAllAsync(from, take);

            AddPaginationHeader(count);
            var model = subscriptions.Select(MapModel);
            return Ok(model);
        }

        private void AddPaginationHeader(int count)
        {
            var total = new KeyValuePair<string, StringValues>("total", new StringValues(count.ToString()));
            Response.Headers.Add(total);
        }

        private static QuerySubscriptionModel MapModel(Subscription subscription)
        {
            var model = new QuerySubscriptionModel
            {
                SubscriptionType = subscription.Type.ToString(),
                SubscriptionStatus = subscription.Status.ToString(),
                UserId = subscription.UserId,
                UserName = $"{subscription.User.Profile.FirstName} {subscription.User.Profile.LastName}",
                Email = subscription.User.Profile.Email,
                CreditCard = subscription.User.Profile.Payment.CreditCard,
                CreditCardType = subscription.User.Profile.Payment.CreditCard
            };
            return model;
        }
    }
}
